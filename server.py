import socket
from datetime import datetime
from decimal import Decimal
import random

UDP_IP = "" 
UDP_PORT = 30663
CONNECTED_PLAYERS = {}
PLAYER_SCORE = {}
AVAILABLE_PLAYER_IDS = ["1","2"]
OBJECTS = {}
coinPoints = 1
counterMessagesRecv = 0
counterMessagesSent = 0
sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))
I = 1

def broadcast(data,addr):
      global counterMessagesSent
      for k, v in CONNECTED_PLAYERS.iteritems():
            if v != addr:
                sock.sendto(data,v)
                counterMessagesSent += 1
                print "Broadcasted position/score/object to network" + str(v)

def reply(data,addr):
      global counterMessagesSent
      sock.sendto(data,addr)
      counterMessagesSent +=1
      print "Replied data to player"

while True:
    if (len(CONNECTED_PLAYERS) == 2):
      if (len(AVAILABLE_PLAYER_IDS) == 0):
          if (len(OBJECTS) < 10):
            print "All players has connected! Start spawning objects"
            x = 1.1 + random.uniform(-54.12668*0.8,54.12668*0.8) # center point(x) + random (+-width* 0.8) 
            y = 47.4 + random.uniform(-53.18718*0.8,53.18718*0.8) # center point(y) + random (+-height* 0.8) 
            z = 59.8 + random.uniform(-53.86607*0.8,-53.86607*0.8) # center point(z) + random (+-length* 0.8)
            x1 = 1.1 + random.uniform(-54.12668*0.8,54.12668*0.8) # center point(x) + random (+-width* 0.8) 
            y1 = 47.4 + random.uniform(-53.18718*0.8,53.18718*0.8) # center point(y) + random (+-height* 0.8) 
            z1 = 59.8 + random.uniform(-53.86607*0.8,-53.86607*0.8) # center point(z) + random (+-length* 0.8)
            objectName = "Key" + str(I)
            objectName1 = "Key" + str(I+1)
            print "Object ",objectName," generated for player at: <", x, y, z, ">" # got coordinates x,y,z of objects
            print "Object ",objectName1," generated for player at: <", x1, y1, z1, ">" # got coordinates x,y,z of objects
            OBJECTS[objectName] = 1
            OBJECTS[objectName1] = 1
            objectLocation = "objectLocation," + objectName + "," + str(x) + "," + str(y) + "," + str(z)
            objectLocation1 = "objectLocation1," + objectName1 + "," + str(x1) + "," + str(y1) + "," + str(z1) 
            sock.sendto(objectLocation,CONNECTED_PLAYERS["Player1"])
            sock.sendto(objectLocation,CONNECTED_PLAYERS["Player2"])
            print "Object",objectName," location sent to ", CONNECTED_PLAYERS["Player1"] , " and ", CONNECTED_PLAYERS["Player2"]
            sock.sendto(objectLocation1,CONNECTED_PLAYERS["Player1"])
            sock.sendto(objectLocation1,CONNECTED_PLAYERS["Player2"])
            print "Object",objectName1," location sent to ", CONNECTED_PLAYERS["Player1"] , " and ", CONNECTED_PLAYERS["Player2"]
            I+=2
    buf = bytearray(1024)
    nbytes, addr = sock.recvfrom_into(buf) # get the size of the received UDP packet
    try:
      data = buf.decode("utf-8")
      print "data rec",data," from",addr
      command = data.split(",")
      counterMessagesRecv +=1
      if command[0] == 'request' :
          if len(AVAILABLE_PLAYER_IDS) > 0:
                temp = AVAILABLE_PLAYER_IDS.pop(0) 
                sock.sendto("createplayer," + temp,addr)
                CONNECTED_PLAYERS["Player" + temp] = addr
                PLAYER_SCORE["Player" + temp] = 0
                print len(CONNECTED_PLAYERS)
                if len(CONNECTED_PLAYERS) > 1:
                      sock.sendto(data,addr)
                      print "sent back--------------"
                print "Sent Create player to client"
                broadcast(data,addr)
      if command[0] == 'register' :
          print "Player registration request received from Address:",addr
          print "Player ",addr," registered with the Game Server"
          print "Player position : <",command[1],",",command[2],",",command[3],">"
          sock.sendto("Registered with server",addr)
          CONNECTED_PLAYERS[str(addr)] = addr
          print "Sent confirmation to client"
          broadcast(data,addr)  
      if command[0] == 'position1+rotation1' :
          start = datetime.now()
          print "Player 1 position : <",command[1],",",command[2],",",command[3],">"
          print "Player 1 rotation : <",command[4],",",command[5],",",command[6],",",command[7],">"
          print "Packet was sent by player 1 at: <",command[8]
          s = str(command[8]).rstrip('\x00') # remove all \x00 from string
          broadcast(data,addr)
          end = datetime.now()
          packetSentTime = float(s)
          packetArrivalTime = float(datetime.now().strftime("%H%M%S.%f")[:-3])
          timediff = packetArrivalTime - packetSentTime
          print "Packet p1 received after: ", timediff, " seconds"
          print "Packet p1 broadcasted after: ", (end - start).total_seconds(), " seconds"
      if command[0] == 'position2+rotation2' :
          start = datetime.now()
          print "Player 2 position : <",command[1],",",command[2],",",command[3],">"
          print "Player 2 rotation : <",command[4],",",command[5],",",command[6],",",command[7],">"
          print "Packet was sent by player 2 at: <",command[8]
          s = str(command[8]).rstrip('\x00') # remove all \x00 from string
          broadcast(data,addr)
          end = datetime.now()
          packetSentTime = float(s)
          packetArrivalTime = float(datetime.now().strftime("%H%M%S.%f")[:-3])
          timediff = packetArrivalTime - packetSentTime
          print "Packet p1 received after: ", timediff, " seconds"
          print "Packet p1 broadcasted after: ", (end - start).total_seconds(), " seconds"
      if command[0] == 'disconnect_one':
          print "Player ",addr," disconnected"
          AVAILABLE_PLAYER_IDS.append("1")
          del CONNECTED_PLAYERS[str(addr)]
          del PLAYER_SCORE[str(addr)]
          raise SystemExit(0)
      if command[0] == 'disconnect_two':
          print "Player ",addr," disconnected"
          AVAILABLE_PLAYER_IDS.append("2")
          del CONNECTED_PLAYERS[str(addr)]
          del PLAYER_SCORE[str(addr)]
          raise SystemExit(0)
      if command[0] == 'request_scores':
          print "Score request received"
          print ""
          if (len(PLAYER_SCORE) == 2):
              if (PLAYER_SCORE["Player1"] == 20):
                  data = "winner," + "Player1," +  str(int(PLAYER_SCORE["Player1"])) + "," + str(int(PLAYER_SCORE["Player2"]))
                  sock.sendto(data,CONNECTED_PLAYERS["Player1"])
                  sock.sendto(data,CONNECTED_PLAYERS["Player2"])
                  print "We found a winner!"
                  print data
              elif (PLAYER_SCORE["Player2"] == 20):
                  data = "winner," + "Player2," +  str(int(PLAYER_SCORE["Player1"])) + "," + str(int(PLAYER_SCORE["Player2"]))
                  sock.sendto(data,CONNECTED_PLAYERS["Player1"])
                  sock.sendto(data,CONNECTED_PLAYERS["Player2"])
                  print "We found a winner!"
                  print data
              else :
                  scorestr = "score," + str(int(PLAYER_SCORE["Player1"])) + "," + str(int(PLAYER_SCORE["Player2"])) 
                  reply(scorestr,addr)
      if command[0] == 'object_destroyed1':
          print "Event request received"
          objectID = str(command[1]).rstrip('\x00') # remove all \x00 from string
          playerID = str(command[2]).rstrip('\x00') # remove all \x00 from string
          for k, v in OBJECTS.items():
            if k == objectID:
              PLAYER_SCORE[playerID] += 1
              del OBJECTS[objectID]
          print OBJECTS
          print "The score of ", playerID, " increased by 1"
          data = "objectDestroyed1," + objectID + "," + playerID
          sock.sendto(data,CONNECTED_PLAYERS["Player2"])
          print "Object destroyed command send to Player 2"
      if command[0] == 'object_destroyed2':
          print "Event request received"
          objectID = str(command[1]).rstrip('\x00') # remove all \x00 from string
          playerID = str(command[2]).rstrip('\x00') # remove all \x00 from string
          for k, v in OBJECTS.items():
            if k == objectID:
              PLAYER_SCORE[playerID] += 1
              del OBJECTS[objectID]
          print OBJECTS
          print "The score of ", playerID, " increased by 1"
          data = "objectDestroyed2," + objectID + "," + playerID
          sock.sendto(data,CONNECTED_PLAYERS["Player1"])
          print "Object destroyed command send to Player 1"
      print "Connected players list", CONNECTED_PLAYERS
      print "Connected players score", PLAYER_SCORE
      print "Available player", AVAILABLE_PLAYER_IDS
      print "Number of Messages Received by Server: ", counterMessagesRecv
      print "Number of Messages Sent by Server: ", counterMessagesSent
      print ""
    except Exception as e:
      print "",e


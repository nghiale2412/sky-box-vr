import socket
from datetime import datetime
from decimal import Decimal

UDP_IP = "" 
UDP_PORT = 30663
CONNECTED_PLAYERS = {}
PLAYER_SCORE = {}
AVAILABLE_PLAYER_IDS = ["1","2"]
coinPoints = 1
counterMessagesRecv = 0
counterMessagesSent = 0
sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

def broadcast(data,addr):
      global counterMessagesSent
      for k, v in CONNECTED_PLAYERS.iteritems():
            if k != str(addr):
                sock.sendto(data,v)
                counterMessagesSent += 1
                print "Broadcasted position/score to network" + str(v)

def reply(data,addr):
      global counterMessagesSent
      sock.sendto(data,addr)
      counterMessagesSent +=1
      print "Replied score to player"

while True:
    buf = bytearray(1024)
    nbytes, addr = sock.recvfrom_into(buf) # get the size of the received UDP packet
    try:
      data = buf.decode("utf-8")
      print "data rec",data
      command = data.split(",")
      counterMessagesRecv +=1
      if command[0] == 'request' :
          if len(AVAILABLE_PLAYER_IDS) > 0:
                temp = AVAILABLE_PLAYER_IDS.pop(0) 
                sock.sendto("createplayer," + temp,addr)
                CONNECTED_PLAYERS[str(addr)] = addr
                PLAYER_SCORE[str(addr)] = 0
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
          start1 = datetime.now()
          print "Player 1 position : <",command[1],",",command[2],",",command[3],">"
          print "Player 1 rotation : <",command[4],",",command[5],",",command[6],",",command[7],">"
          print "Packet was sent by player 1 at: <",command[8]
          s1 = str(command[8]).rstrip('\x00') # remove all \x00 from string
          broadcast(data,addr)
          end1 = datetime.now()
          packetSentTime1 = float(s1)
          packetArrivalTime1 = float(datetime.now().strftime("%H%M%S.%f")[:-3])
          timediff1 = packetArrivalTime1 - packetSentTime1
          print "Packet p1 received after: ", timediff1, " seconds"
          print "Packet p1 broadcasted after: ", (end1 - start1).total_seconds(), " seconds"
      if command[0] == 'position2+rotation2' :
          start2 = datetime.now()
          print "Player 2 position : <",command[1],",",command[2],",",command[3],">"
          print "Player 2 rotation : <",command[4],",",command[5],",",command[6],",",command[7],">"
          print "Packet was sent by player 2 at: <",command[8]
          s2 = str(command[8]).rstrip('\x00') # remove all \x00 from string
          broadcast(data,addr)
          end2 = datetime.now()
          packetSentTime2 = float(s2)
          packetArrivalTime2 = float(datetime.now().strftime("%H%M%S.%f")[:-3])
          timediff2 = packetArrivalTime2 - packetSentTime2
          print "Packet p1 received after: ", timediff2, " seconds"
          print "Packet p1 broadcasted after: ", (end2 - start2).total_seconds(), " seconds"
      if command[0] == 'score1' :
          print "Player 1 score: <", command[1],">"
          broadcast(data,addr)
      if command[0] == 'score2' :
          print "Player 2 score: <", command[1],">"
          broadcast(data,addr)
      if command[0] == 'disconnectone':
          print "Player ",addr," disconnected"
          AVAILABLE_PLAYER_IDS.append("1")
          del CONNECTED_PLAYERS[str(addr)]
          del PLAYER_SCORE[str(addr)]
      if command[0] == 'disconnecttwo':
          print "Player ",addr," disconnected"
          AVAILABLE_PLAYER_IDS.append("2")
          del CONNECTED_PLAYERS[str(addr)]
          del PLAYER_SCORE[str(addr)]
      #if command[0] == 'ScoreObject':
      #    print "Player request received"
      #    score = PLAYER_SCORE[str(addr)]
      #    PLAYER_SCORE[str(addr)] = score+coinPoints
      #    scorestr = "score,"+str(PLAYER_SCORE[str(addr)])
      #    reply(scorestr,addr)
      print "Connected players list", CONNECTED_PLAYERS
      print "Connected players score", PLAYER_SCORE
      print "Available player", AVAILABLE_PLAYER_IDS
      print "Number of Messages Received by Server: ", counterMessagesRecv
      print "Number of Messages Sent by Server: ", counterMessagesSent
    except Exception as e:
      print "",e


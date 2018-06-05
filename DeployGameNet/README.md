## Steps for Deploying GameNet to any kubernetes cluster

GameNet is a cross-platform game networking library written to work on Raspberry Pi. You can also use this library in any platform with Python installed.

### Starting the deployment

Following docker run command deploys the container,

```markdown
>> docker run -v <<local .kube folder location>>:/root/.kube -it docker.io/cegvinoth/deploygamenet
```

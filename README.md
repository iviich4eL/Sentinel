# How to run sample

You need pre-installed docker and docker-compose.

For successful testing on a local machine you need a bridge network and a fake api.

0. Clone this repository

1. Create bridge network

```
docker network create -d bridge my-network
```

2. Build fake data api
```
docker build --no-cache -t fakeapi -f .\Sentinel\FakeDataApi\Dockerfile .
```

3. Build sentinel
```
docker build --no-cache -t sentinel -f .\Sentinel\Server\Dockerfile .
```

4. Run
```
docker-compose up -d
```

5. Webassembly app is on localhost (enter 'localhost' in browser). Api is on 5002 port.
```
localhost
http://localhost:5002/data
```

To run containers without docker compose separately

```
docker run -dp 5002:5002 fakeapi
docker run -dp 80:80 sentinel
```

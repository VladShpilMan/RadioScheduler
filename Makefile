.PHONY: build run test docker-build docker-run docker-test

build:
	dotnet build

run:
	dotnet run --project RadioScheduler.Api

test:
	dotnet test --logger "console;verbosity=detailed"

docker-build:
	docker build -t radioscheduler-api .

docker-run:
	docker run --name radioscheduler-container -p 8080:8080 radioscheduler-api
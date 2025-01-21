# CoreDNS

https://dev.to/robbmanes/running-coredns-as-a-dns-server-in-a-container-1d0

---

### How to test through docker-compose

1. `docker-compose up -d --build`
2. `docker compose exec -it ubuntu-dns bash`
3. `apt-get update && apt-get install -y dnsutils`
4. `nslookup myinternalapi.com coredns`

### How to test manually

1. docker run -d --name ubuntu-dns --cap-add=SYS_ADMIN --cap-add=NET_ADMIN ubuntu sleep infinity
2. docker exec -it ubuntu-dns bash
3. apt-get update && apt-get install -y dnsutils vim => 2, 134, 89
4. echo "nameserver 127.0.0.1#1053" > /etc/resolv.conf
   4.1 or `vim /etc/resolv.conf`
5. dig @127.0.0.1 -p 1053 myinternalapi.com

```
nslookup myinternalapi.com 127.0.0.1:1053
dig @127.0.0.1 -p 1053 myinternalapi.com
```

### ECS

- Create an IAM user with the following permissions, and then create its access key
  - `AmazonECS_FullAccess`
  - `AmazonVPCFullAccess`
  - `IAMFullAccess`
  - `CloudWatch` `logs:*`
- Configure it using `aws configure` (needs AWS CLI to be installed)
- Chose one option bellow and use terraform to apply and create the resources
- Test using a local container pointing to ECS FARGATE task
  - `docker run --rm -d -it --name ubuntu --dns="<IP>" ubuntu sleep infinity`
  - `docker exec -it ubuntu bash`
  - `apt update && apt install dnsutils curl -y`
  - `curl google.com`

#### ECS Fargate (x86 && arm64)

Note: `arm64` is getting the error `exec /coredns: exec format error`

- `terraform init`
- `terraform apply`

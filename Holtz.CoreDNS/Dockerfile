FROM coredns/coredns:1.9.4

# Copy Corefile into the image
COPY Corefile /etc/coredns/Corefile

# Set the working directory
WORKDIR /etc/coredns

# Command to start CoreDNS
CMD ["-conf", "/etc/coredns/Corefile"]

# docker build -t custom-core-dns .
# docker run -d --name=core-dns custom-core-dns
# docker run -d --name coredns -p 1053:1053/udp -p 1053:1053/tcp custom-core-dns
# docker run -d --name coredns -p 53:53/udp -p 53:53/tcp custom-core-dns
# docker run -d --name my-container --dns 127.0.0.1 -p 1053:1053/udp ubuntu

# Docker Hub:
# docker build -t holtz-coredns .
# docker tag holtz-coredns henriqueholtz/holtz-core-dns
# docker push henriqueholtz/holtz-core-dns
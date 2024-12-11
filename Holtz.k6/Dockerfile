FROM grafana/k6:master-with-browser

USER root
COPY script.js .

RUN apk add --no-cache coreutils
CMD ["tail", "--version"]
ENTRYPOINT ["tail", "-f", "/dev/null"]
 
# docker build -t holtz-k6 -f ./Dockerfile .
# docker run --rm --name holtz-k6 -it -d holtz-k6 tail -f /dev/null 
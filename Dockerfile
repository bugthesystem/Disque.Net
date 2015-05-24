FROM debian:stable

RUN \
  apt-get update && \
  apt-get install -y --no-install-recommends \
  curl \
  ca-certificates \
  unzip \
  build-essential && \
  rm -rf /var/lib/apt/lists/*
  

RUN \
  cd /tmp && \
  curl https://codeload.github.com/antirez/disque/zip/master -o disque.zip && \
  unzip disque.zip && \
  cd disque-master && \
  make all

ENV PATH /tmp/disque-master/src:$PATH

EXPOSE 7711
CMD ["disque-server"]

FROM node:18-buster

RUN npm install -g @usebruno/cli
COPY . .
RUN chmod +x /run-collection.sh

ENTRYPOINT ["/bin/bash", "/run-collection.sh"]


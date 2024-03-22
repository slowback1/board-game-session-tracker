FROM node:18-buster AS setup
WORKDIR app
COPY package.json .
COPY package-lock.json .
RUN npm ci
COPY . .

FROM setup as test
ENTRYPOINT ["npm", "run", "test:ci"]

FROM setup as builder
RUN npm run build:ci

FROM node:18-alpine as final
COPY --from=builder app/build build/
COPY --from=builder app/node_modules node_modules/
COPY package.json .
ENV NODE_ENV=production
EXPOSE 3000
CMD ["node", "build"]

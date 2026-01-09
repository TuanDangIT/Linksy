FROM node:22-alpine AS builder

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build -- --configuration=production

FROM node:22-alpine

WORKDIR /app

ENV NODE_ENV=production
ENV PORT=4200

COPY package*.json ./
RUN npm ci --omit=dev && npm cache clean --force

COPY --from=builder /app/dist ./dist

RUN addgroup -g 1001 -S nodejs && \
    adduser -S angular -u 1001 && \
    chown -R angular:nodejs /app

USER angular

EXPOSE 4200

CMD ["node", "dist/Linksy.FrontEnd/server/server.mjs"]
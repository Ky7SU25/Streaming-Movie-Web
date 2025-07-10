#!/bin/bash
set -e

# cd về thư mục chứa script
cd "$(dirname "$0")"

# load env
if [ -f .env ]; then
    source .env
else
    echo ".env not found in $(pwd)"
    exit 1
fi

PREV_IMAGE_FILE="/tmp/${APP_NAME}_prev_image"

echo "==> Pulling latest image"
if docker-compose pull; then
    echo "==> Saving current image"
    CURRENT_IMAGE=$(docker inspect --format='{{.Config.Image}}' ${APP_NAME} || echo "")
    echo "$CURRENT_IMAGE" > "$PREV_IMAGE_FILE"

    echo "==> Starting new container"
    docker-compose up -d
else
    echo "==> Failed to pull. Rolling back…"
    if [ -f "$PREV_IMAGE_FILE" ]; then
        PREV_IMAGE=$(cat "$PREV_IMAGE_FILE")
        echo "==> Rolling back to $PREV_IMAGE"
        sed -i "s|image:.*|image: $PREV_IMAGE|" docker-compose.yml
        docker-compose up -d
        echo "Rollback complete"
    else
        echo "No previous image. Cannot rollback."
        exit 1
    fi
fi
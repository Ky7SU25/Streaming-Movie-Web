pipeline {
    agent any

    environment {
        APP_NAME = "streaming-movie-web-web"
        REGISTRY = "103.109.187.143:5001/nguyendat"
        IMAGE = "${REGISTRY}/${APP_NAME}"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'git@github.com:Ky7SU25/Streaming-Movie-Web'
            }
        }
    }
}

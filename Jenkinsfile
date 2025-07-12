pipeline {
    agent any

    environment {
        APP_NAME = "streaming-movie-web-web"
        REGISTRY = "103.109.187.143:5001/nguyendat"
        IMAGE = "${REGISTRY}/${APP_NAME}"
    }

    stages {
	stage('Cleanup') {
            steps {
	        script {
                    sh """
                        cp /var/lib/jenkins/configs/cleanup.sh .
                        chmod +x cleanup.sh
                        ./cleanup.sh
                    """
                }
            }
        }

        stage('Checkout') {
            steps {
                git branch: 'main', url: 'git@github.com:Ky7SU25/Streaming-Movie-Web'
            }
        }

        stage('Build Image') {
            steps {
                script {
                    COMMIT = sh(script: 'git rev-parse --short HEAD', returnStdout: true).trim()
                    IMAGE_TAG = "${IMAGE}:${COMMIT}"
                    echo "Building image: ${IMAGE_TAG}"
                    docker.build(IMAGE_TAG)
                }
            }
        }

        stage('Push Image') {
            steps {
                script {
                    sh """
                        docker tag ${IMAGE}:${COMMIT} ${IMAGE}:latest
                        docker push ${IMAGE}:${COMMIT}
                        docker push ${IMAGE}:latest
                    """
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    def result = sh(script: """
                        cp /var/lib/jenkins/configs/deploy.sh .
                        chmod +x deploy.sh
                        ./deploy.sh
                    """, returnStatus: true)
                    if (result != 0) {
                        error "Deploy failed. Rollback attempted."
                    }
                }
            }
        }
    }
}

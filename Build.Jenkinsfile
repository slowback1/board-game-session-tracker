pipeline {
        environment {
            REGISTRY = "${env.DOCKER_REGISTRY}"
            DOCKERHUB_CREDENTIALS_ID = "docker_credentials"
            GIT_REPOSITORY = "https://github.com/slowback1/board-game-session-tracker.git"
        }

        agent any

        stages {
            stage("Fetch Git Repository") {
                steps {
                    git GIT_REPOSITORY
                }
            }

            stage("Build and Push Docker Images") {
                steps {
                    container("docker") {
                        docker.withRegistry(REGISTRY, DOCKERHUB_CREDENTIALS_ID) {
                            sh "./frontend/scripts/build-docker-image.sh"
                            sh "./api/scripts/build-docker-image.sh"
                        }
                    }
                }
            }
        }

}
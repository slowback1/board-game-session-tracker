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
                        withCredentials([usernamePassword(credentialsId: DOCKERHUB_CREDENTIALS_ID, passwordVariable: 'DOCKER_REGISTRY_PWD', usernameVariable: 'DOCKER_REGISTRY_USER')]) {
                            sh "docker login -u ${DOCKER_REGISTRY_USER} -p ${DOCKER_REGISTRY_PWD}"
                            sh "./frontend/scripts/build-docker-image.sh"
                            sh "./api/scripts/build-docker-image.sh"
                        }
                    }
                }
            }
        }

}
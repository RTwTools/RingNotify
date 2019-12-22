pipeline
{
  agent
  {
    docker { image 'mcr.microsoft.com/dotnet/core/sdk:3.1' }
  }
  options
  {
    skipDefaultCheckout true
  }
  environment
  {
    DOTNET_CLI_HOME = "/tmp/DOTNET_CLI_HOME"
  }
  stages
  {
    stage('Clean')
    {
      steps
      {
        deleteDir()
      }
    }
    stage('Checkout')
    {
      steps
      {
        checkout scm
      }
    }
    stage('Build')
    {  
      steps
      {
        sh 'bash build.sh'
      }
    }
    stage('Publish')
    {
      steps
      {
        sh 'bash publish.sh'	
      }
    }
  }
  post
  {
    always
    {
      archiveArtifacts artifacts: "release/**/*"
    }
  }
}
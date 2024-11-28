# PicusApp
------------------------------------------
This repository is a PicusApp that performs simple CRUD operations in AWS DynamoDB using dotnet 8.0.  Also contains the CI/CD pipeline configuration for the PicusApp project. The pipeline automates the build, test, and deployment processes, ensuring seamless application delivery.

The application is deployed to AWS EC2 with the help of pipeline.

*******************************************
AWS Resources
- IAM
- ECR
- ECS
- EC2
- DynamoDB
- Lamda Functions
------------------------------------------


GitHub Secrets:
- Ensure the following secrets are configured in your GitHub repository:

  - AWS_DYNAMO_DB_ACCESS_KEY_ID
  - AWS_SECRET_DYNAMO_DB_ACCESS_KEY
  - AWS_REGION
  - AWS_TABLE_NAME
  - AWS_ACCESS_KEY_ID
  - AWS_SECRET_ACCESS_KEY
  - AWS_ECR_URI
  - EC2_HOST
  - EC2_USER
  - EC2_KEY
  - ELASTIC_LOADBALANCER_LISTENER_ARN
  - ELASTIC_LOADBALANCER_TARGET_GROUP_ARN_8080
  - ELASTIC_LOADBALANCER_TARGET_GROUP_ARN_8081

------------------------------------------
How to Use
1) Push to master Branch
Trigger the pipeline by pushing changes to the master branch.

2) Rollback Deployment
The pipeline automatically supports rollback in case of issues during deployment.

3) Health Check
The pipeline performs health checks post-deployment and ensures the app is healthy.

------------------------------------------
Pipeline Summary:

  - Build:	Builds the application, creates Docker images, and pushes to AWS ECR.
  - CreateRollbackImage:	Deploys a rollback container for fallback scenarios.
  - ReplaceLBTargetGroupTo8081:	Switches traffic to rollback container.
  - Deploy:	Deploys the latest application version.
  - ReplaceLBTargetGroupTo8080:	Switches traffic to the new application version.
  - DeleteRollbackImage:	Cleans up rollback resources.

------------------------------------------
Dependencies
* AWS CLI: Installed and configured on the target EC2 instance.
* Docker: Installed on both the build server and EC2 instance.
* GitHub Actions: Configured with the provided workflow file

-----------------------------------------------------------



**Notes:
Ensure all required GitHub secrets are correctly set up before running the pipeline.
Modify the appsettings.json placeholder values to match your environment's configuration.**

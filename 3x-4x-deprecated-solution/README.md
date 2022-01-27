# Dynamics 365 Project Service Automation 3x to Project Operations 4x deprecated components

> NOTE: This is not supported in production environments and is provided as a convience for developers only.


This solution adds the deprecated components to an installation of Project Operations in order to allow the import of 3.x customizations. This allows customers and ISV's to import their customizations developed in Project Service Automation into Project Operations and work on migrating to the new model.

# Usage
1. In an environment installed with Dynamics 365 Project Operations install this [solution](https://github.com/microsoft/Dynamics365-Project-Operations-PowerApps/raw/main/3x-4x-deprecated-solution/msdyn_ProjectServiceDeprecatedComponents_managed.cab).
2. Install your customizations from PSA into Project Operations
3. Proceeed to updated your customizations to the new schema, you are able to determine dependencies by attempting to uninstall the deprecated solution. If there are dependencies then the platform will error out on uninstall and you are able to address those.
4. Remove the deprecated components soltuion.

# License
The CAB file and resulting code is supplied to customers licensed for Dynamcis 365 Project Operations only. This is not covered by the MIT license.


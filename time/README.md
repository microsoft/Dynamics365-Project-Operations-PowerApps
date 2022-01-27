# Dynamics 365 Project Operations Time Entry (Canvas Mobile) Sample

> Applies to:
> - Project Operations Lite Deployment
> - Project Operations Integrated Deployment
>
> See [deployment types](https://docs.microsoft.com/en-us/dynamics365/project-operations/environment/determine-deployment-type#deployment-types) for more information.


The Time Entry sample canvas app provides access for project team members to create and manage their time entries. For more examples and contribution notes please read the [repository README](https://github.com/microsoft/Dynamics365-Project-Operations-PowerApps).

![Period view](https://github.com/microsoft/Dynamics365-Project-Operations-PowerApps/blob/main/images/time-period.png?raw=true) ![Time line view](https://github.com/microsoft/Dynamics365-Project-Operations-PowerApps/blob/main/images/time-timeline.png?raw=true)

**[Download Current Package Here](https://github.com/microsoft/Dynamics365-Project-Operations-PowerApps/raw/main/time/package/TimeEntry_20220126221310.zip)**

**Features include:**
- Weekly timeline view
- Create, Edit, Submit and Recall time entries on multiple days
- Draft/Returned indicator
- Bulk operations
- Role Selection Defaulting
- Extensible
- Correct timezone handling relative to personalization settings within Dataverse

## Table of Contents
1. [Installation](#installation)
2. [Prerequisites](#prerequisites)
3. [Instuctions for package install](#instructionsforcompletepackate)
4. [Usage](#usage)
5. [Known Issues](#knownissues)
6. [Development Notes](#developmentnotes)
7. [Support](#support)


# Installation

## Prerequisites
- An environment with **Project Operations Integrated** or **Lite deployment**, see [deployment types](https://docs.microsoft.com/en-us/dynamics365/project-operations/environment/determine-deployment-type#deployment-types) for more information.
- A user with administrative priviledges and a Project Operations license assigned
- (Recommended) A user account for use to connect PowerAutomate Dataverse Connector to Project Operations (service account), this is to ensure that the app will continue to work should the installing administrator account be retired.

## Instructions for package install
1. Download the current package from the link at the top of the page.
2. As the administrator open https://make.powerapps.com
3. Select the environment containing Project Operations from the environment picker in the header (top right)
4. Select **Apps** on the left and press **Import canvas app**

## Usage

### Navigating the period
The app is designed to be simple and intuitive to use, start by selecting the period you wish to work in using either the date picker or arrows in the header.

### Searching witin the period
Searching is currently only supported for the selected period. Typing text in will search the project or task. Clear the search box to return all records.

### Press the + button to add a time entry
Time entries are specified as a date and duration, you are able to select multiple days for creating entries that repeat in the week.

### Editing the time entry
Selecting the time entry will allow you to view the details of the entry. If the entry is in DRAFT state then the app will allow you to update and end the record. Press the pen icon in the top right to enter edit mode.

### Submitting time entries
The app allows the submission of multiple time entries

## Known Issues
- Apporovals not yet supported (may be presented in a different app)
- Online connectivity required
- Record limit of 500
- Localization - labels are in English only
- Recalling does not prompt the user for a reason

# Development Notes

## What is a time entry
A time entry if a block of time defined with a start, end and duration. When creating a time entry the system will enforce that these values are in alignment. Time entries include a type field 'Time Entry Source' which is used to segregate time entries into different applications. This sample only supports Project Operations time entries.

## Time zone implication for Time Entries
Date and time fields are stored within Dataverse in UTC, Dataverse also stores the user's set time zone. When reading data, creating data and working with data that involves the time zone, you will need to take into account the user's settings in Dataverse for their time zone. This app calcualtes the relative difference between the user's browser time zone and their set time zone in the personalization settings in Dataverse.

## Defaulting the Role
The role of the entry determines the pricing used when calculating the sales price of the work performed.
By default when selecting the Project the app we assign the bookable resources' (current user's) default role within the project. If a task is selected and the task has an assigned role, then the role is updated to reflect the task's role.

# Support
This app is provided as is and intended to be an example on how you can leverage the PowerPlatform with your purchased Project Operations licenses. Microsoft does not support this app directly, this app is supported through volenteers who 
# PiRhoUtilities Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
### Removed
### Changed

## [2.1.0] - 2020-1-28
### Changed
- Upgraded project to be compatible with the final release of Unity 2019.3

## [2.0.0] - 2019-12-31
> This is a large, non backwards compatible API update. Most fields and attributes have been changed or have had additional functionality added and not all changes/fixes are listed below.
### Added
- Ability to use reflection sources for most properties on attributes including auto-updating them or binding to their SerializedProperty value
- UXML support for most controls
- Better samples for each of the Attributes
- Code and UXML example window
### Removed
- RolloutAttribute in favor of "IsCollapsable" on Frame
### Changed
- All "Control" classes have now been merged into their respective field classes to more match Unity's internal API.
- Fixed drag and drop for pickers
- Better history window/styling
- Popup now wraps Unity's to handle awkward setting of values that are not in the list without throwing exceptions and breaking the inspector

## [1.2.0] - 2019-12-11
### Added
- Added the ability to specify a method, property, or field as the values and options for PopupAttribute
- Added Test Assemblies
- Added TabsAttribute and corresponding control to show and hide fields via tabs groups
### Removed
- Removed the dependency to Addressables and the ScenePickerAttribute and corresponding controls
### Changed
- Made Picker's style match that of a normal dropdown
- Fixed situations for multiple attributes where callbacks on private properties would throw a null reference
- Fixed Placeholders with delayed TextFields
- Fixed ComboBox styling
- Fixed Foldout icon on high DPI monitors
- Fixed Samples.meta warning when installing package via git

## [1.1.1] - 2019-10-24
### Added
- Added an generic extension method for SetLabel on BaseField
### Changed
- Make it so SerializedDictionary lists stay in memory during runtime so that they can be accessed by serialized properties
- Changed so the ConfigureProperty call automatically gets the tooltip instead of having to pass it in

## [1.1.0] - 2019-10-23
### Added
- Added a NoLabel attribute which removes the label from a BaseField<> or PropertyField
- Added SerializedProperty constructors for each of the controls so that they can be created and automatically be bound to properties
- Added DragAndDrop functionality for Pickers and the History window
- Added an extension method for SerializedProperty to retreive the tooltip attribute defined it's field
- Added the ability to declary maximums and minimums for various attributes as defined in fields, methods, or properties
### Changed
- Custom label now affects all class names defined in the drawer. This includes Frame's label so that Lists and Dictionaries are now affected

## [1.0.5] - 2019-10-22
### Added
- Added a public getter for the Control on all BaseField<> classes

## [1.0.4] - 2019-10-21
### Changed
- Made Rollout button only clickable from the Icon due to UIElements triggering it when clicking on the Add Key text

## [1.0.3] - 2019-10-14
### Added
- Added support for SerializedDataList

## [1.0.2] - 2019-10-08
### Changed
- Made setting the text of a MessageBox not trigger a change event (Label does this internally for bindings it seems)

## [1.0.1] - 2019-10-08
### Changed
- Fixed Configuration path

## [1.0.0] - 2019-10-07
### Added
- First official release

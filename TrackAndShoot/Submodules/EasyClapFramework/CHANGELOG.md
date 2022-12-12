# Upcoming

## Removals

## Changes

## Additions

## Fixes

## Dependencies

## Internal



# 5.5.0 - 2022-03-10
- Finite State Machines.
- IReadOnlyList<T> extensions.

## Changes
1. Current index overload of GetNextIndexCircular extension now uses WrapIndex instead of the built-in modulo operator.

## Additions
1. Core FSM framework: FSM namespace.
1. MonoBehaviour-based FSM implementation: FSM.MonoBehaviourBased namespace.
1. MultiInstanceModule-based FSM implementation: FSM.MultiInstanceModuleBased namespace.
1. GetNextIndexCircular extension overload that takes in current item.
1. GetNextItemCircular extensions with overloads that take in a current item or current index.
1. WrapIndex extension: Wraps the given index along the length of the enumerable.
1. ModuloNonNegative extensions: Takes modulo of the given number. Output is non-negative.
1. IReadOnlyList<T> extensions:
	- IndexOf
	- GetNextIndexCircular with overloads that take in a current item or a current index
	- GetNextItemCircular with overloads that take in a current item or a current index
1. Extensions for starting a coroutine:
	- StartCoroutineIn: Starts the coroutine in a specific host.
	- StartCoroutineInGlobalSurrogate: Starts the coroutine in GlobalMonoBehaviourSurrogate.
	- StartCoroutineInSceneSurrogate: Starts the coroutine in SceneMonoBehaviourSurrogate.
1. ContainsIndex extension: Returns whether the given index is in range of the enumerable.
1. ReferenceEqualityComparer: An IEqualityComparer that compares operands based solely on reference equality.

## Internal
1. Add StateMachineDrawer: FoldoutDrawer for anything deriving from IStateMachine.
1. Add details optional param to SenecaCommonException.FromArgumentOutOfRange.
1. Add SenecaCommonException.FromNotSupported.



# 5.4.0 - 2022-02-15
- Ability to load the first levels of the sessions in standalone-scene-based level setups directly after the splash screen.

## Additions
1. Splash Screen: More instantiation timing options
	- Before logo fade-in
	- Before logo fade-out
	- After logo fade-out
	- (After logo fade-in already existed before)
1. Level Management: First Level Loader prefab.
1. Splash Screen: Splash screen's scene loader can now be disabled in the config.
1. Splash Screen: SplashScreenLifetimeStage enum. Indicates a certain stage in Splash Screen lifetime.
1. Splash Screen: SplashScreenLifetimeEvent event. Indicates that the Splash Screen has reached a certain stage of its lifetime. Emitted by the framework.

## Internal
1. Splash Screen: SplashSceneLoadBehaviour enum.
1. Splash Screen: Reorder splash screen logic for better responsiveness in some areas.
1. Splash Screen: Add SplashScreenConfigModule.GetPrefabsToInstantiate method.



# 5.3.0 - 2022-02-09
- Level Loader SmartLoad method.

## Additions
1. Level Management: Level Loader SmartLoad method: Loads the most appropriate level depending on the current state of the framework.
1. Level Management: Level Loader HasFirstLevelOfTheSessionLoaded property.
1. Level Management: Progression Manager WasLatestCompleteASuccess property.
1. Level Management: Progresssion Manager HandleLevelComplete method canSetWasLatestCompleteASuccess optional parameter.

## Internal
1. Level Management: Progression Manager use HasFirstLevelOfTheSessionLoaded of the active level loader.



# 5.2.0 - 2022-02-08

## Changes
1. Level Management: Level Loader default values for LevelLoadMethodAfterCompletion.

## Additions
1. Level Management: Level Loader virtual level index persistence option.



# 5.1.0 - 2022-02-05

## Changes
1. Level Management: Level Loader safety guards against out of bounds level index.
1. Level Management: Level Database safety guard against empty level array.

## Additions
1. Splash: Support for loading the scene with build index 1. This is enabled by default, since most projects place the main scene at build index 1.

## Fixes
1. Level Management: LoadNextLevel was declaring a reload.

## Internal
1. Splash: Refactor.



# 5.0.0 - 2022-02-02
- Snapshotting utilities, along with lerping and tweening support.
- Streamlined level management.

## Changes
1. **BREAKING CHANGE: ModuleManagement: InitDerived is renamed to Init.**
1. DestroyOnInit component now inherits from InitActionMonoBehaviour

## Removals
1. **BREAKING CHANGE: Analytics events (and the Integrations namespace). Analytics tools are now expected to use the Level Management events.**

## Additions
1. Snapshot struct: A specific orientation in space. Consists of a position, a rotation, and a scale. Intended to store transform orientations as a whole.
1. SnapshotTweenBuilder struct: Builder for a DOTween Sequence which can selectively tween values of a Transform to a Snapshot.
1. SnapshotTweenConfig class: Configuration for a DOTween Sequence which can selectively tween values of a Transform to a Snapshot.
1. Extensions for interop between a Transform and a Snapshot:
	- TakeSnapshot
	- SetFromSnapshot
	- Set(Position|Rotation|LocalScale)FromSnapshot
1. Extensions for lerping a transform to a Snapshot:
	- Lerp
	- Lerp(Position|LocalScale)
	- SlerpRotation
1. New custom DOTween shorthands for tweening a Transform to a Snapshot: 
	- DOOrient
	- DOOrient(Position|Rotation|Scale)
1. Overloads for some built-in DOTween shorthands to take in a Snapshot:
	- DO(Local)Move
	- DO(Local)RotateQuaternion
	- DOScale
1. Editor: FoldoutDrawer class: Base class for a property drawer that, when derived from, wraps the property in a foldout.
1. Editor: BoxDrawer class: Base class for a property drawer that, when derived from, wraps the property in a box.
1. Editor: Indentation utilities:
	- IndentByOneContext class: A using context that indents the content by 1 compared to where it should normally appear.
	- IndentContext class: A using context that sets the indent level of its content to the given level.
	- SenecaEditorUtility.PushIndentByOne method
	- SenecaEditorUtility.PopIndentLevel method alias
1. Surrogates for null conditional operators:
	- Maybe extension
	- Maybe(GameObject|Transform|GetComponent) extensions
1. TwoWayTweener component: A component that can tween its transform between two orientations (Snapshots).
1. PlayerPrefsEntry drawers, with editing support.
1. BoxDrawer & FoldoutDrawer: support for custom header content.
1. FoldoutDrawer: expose IsFoldoutExpanded.
1. Nicify editor extension: alias for ObjectNames.NicifyVariableName
1. Editor GuiEnabledMementoContext: A using context that remembers GUI.enabled on enter, and restores it on exit.
1. Editor LabelWidthMementoContext: A using context that remembers EditorGUIUtility.labelWidth on enter, and restores it on exit.
1. LevelManagement PlayerPrefs LastSuccessfulLevelIndex & DidCompleteAtLeastOnce.
	- PlayerPrefsGateway contatining these two.
	- Key constants in Constants file.
1. LevelManagement: Level databases
	- ILevelDatabase interface: Non-generic interface for level databases.
	- LevelDatabase base module: Base class for all level databases that intend to integrate with the framework's level management.
	- PrefabLevelDatabase module: A pre-made level database for the prefabs-based level setups (levels are prefabs).
	- SceneLevelDatabase module: A pre-made level database for the scene-based level setups (levels are scenes).
1. LevelManagement: Level loaders
	- ILevelLoader interface: Non-generic interface for level loaders.
	- LevelLoader base module: Base class for all level loaders that intend to integrate with the framework's level management.
	- PrefabLevelLoader module: A pre-made level loader for the prefabs-based level setups (levels are prefabs).
	- SceneLevelLoader module: A pre-made level loader for the scene-based level setups (levels are scenes). Works with both standalone levels, and additive levels with a central level loader.
	- LevelLoadMethodAfterCompletion enum: The level selection behaviour to follow after the player has completed all the levels.
1. LevelManagement: ProgressionManager module: Handles everything related to level-to-level progression.
1. LevelManagement events:
	- LevelLoadStartedEvent: Indicates a level has just started loading. Emitted by framework.
	- LevelLoadCompletedEvent: Indicates a level is loaded successfully. Emitted by framework.
	- LevelCompletedEvent: Indicates that a level has been completed. Emitted by framework.
1. LevelServiceLocator module: A service locator for the level management. Provides non-generic access to the active instances of a level database, a level loader, and a progression manager.
1. LoadFirstLevelOnInit component: Calls LoadFirstLevel on the active level loader at the beginning of the component lifecycle.
1. ModuleManagement: Module.InitSecondPass method: This is a second pass for the Init method. All Init methods will be invoked before the second pass.
1. PlayerPrefsEntryBool class: PlayerPrefsEntry for boolean values.
1. ComponentInitEventType enum
1. InitActionMonoBehaviour base component: Base class for a MonoBehaviour that runs an action at the beginning of the component lifecycle.
1. TryGetClosedGenericAncestorType extension: Searches upwards to find a closed generic type that matches a given open generic type in the hierarchy of a given type.
1. GlobalMonoBehaviourSurrogate & SceneMonoBehaviourSurrogate: No-op MonoBehaviour singletons. Can be used as coroutine hosts.
1. Utils.GetRandomIntExcept method: Return a random integer with the guarantee that it will not be equal to a certain value or values.
1. Utils.ConditionalGenerate method: Keeps generating a value until a pass condition is met.

## Fixes
1. BoxDrawer & FoldoutDrawer: Added a guard against null labels. Manually replaces null labels with GUIContent.None.

## Internal
1. Constants are now PublicAPI as a whole.
1. ModuleManagement: Init is renamed to Bootstrap.
1. ModuleManagement: Add a second pass for module bootstrap.



# 4.0.0 - 2021-12-30

## Removals
1. **BREAKING CHANGE: EventBus editor support: SerializedEvent class is removed due to instability of T4 code generation.**

## Dependencies
1. **BREAKING CHANGE: Remove: com.faster-games.t4**



# 3.1.0 - 2021-12-28
Switched from UPM distribution to `.unitypackage` distribution.

## Additions
1. JoinToString extension.
1. EventBus editor support: SerializedEvent class.

## Dependencies
1. New: com.faster-games.t4 1.4.1

## Internal
1. Use names instead of GUIDs for asmdefs.
1. Reference Scripts asmdef in Editor asmdef.
1. Add an assembly tunnel (asm ref) for EasyClap.Games assembly.



# 3.0.1 - 2021-12-19

## Internal
1. Changed all dependency channels from OpenUPM to Git SSH URLs.
1. Removed the sandbox dependencies which were already defined as package dependencies.



# 3.0.0 - 2021-12-17

## Changes
1. **BREAKING: MultiInstanceModule now requires the module type as its generic argument.**
1. **BREAKING: ReplaceWithPrefabTool.NamingMethod is now private (was public).**

## Additions
1. AnalyticsLevelStartedEvent & AnalyticsLevelCompletedEvent.
1. GetLevelName utility.
1. StartupConfigurator & StartupConfigurationPreset.
1. PlayerPrefs entry wrappers.
1. Uitlities for editor-time module resolution.
1. DestroyOnInit component.
1. Extensions for Lerp & Slerp targeting a transform.
1. ResetPosRot & ResetLocalPosRot extensions.
1. GetNextIndexCircular extension.
1. HasSameElementsWith extension.
1. Identity function & extension for ordering an IEnuemrable with identity function.
1. WithAlpha extension on Color.
1. SetColorAlpha extension on Image.
1. Decontructor for KeyValuePair as an extension.
1. MoveTowards extension.
1. WhereNotNull linq-alike extension.
1. LinqToUnity extensions: SelectComponent, FirstComponent, FirstComponentOrDefault.
1. GetRandomItem & GetRandomIndex extensions.
1. Repeat extension.
1. EnglishAlphabetUppercase static char array. GetRandomUppercaseEnglishLetter and GetRandomUppercaseString utilities.
1. ContainsLayerRaw & ContainsLayerShifted extensions.
1. ConvertRange, ConvertRangeFrom01, ConvertRangeTo01, NormalizeRange extensions.
1. TriggerEventAdapter component.
1. LinqToUnity extensions: SelectClosest, SelectFarthest.
1. SelectionUtilties.SelectFittest as a general puspose criteria-based selection utility.
1. LookAtTool editor utility.
1. UGuiTools editor utilities.
1. ToObjectSummaryString extension.
1. EventBus can now log arguments as well.

## Fixes
1. EventBus now accomodates listeners being removed during an event invocation.
1. Unintended integer division in SenecaEditorUtility.DrawLine.

## Internal
1. Decorated public APIs with PublicAPI attribute.
1. There is no longer an assembly-wide Preserve. Only the types that need to be preserved are marked as such.
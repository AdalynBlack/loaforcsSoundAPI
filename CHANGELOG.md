## v1.1.0
- Looping sounds *should* now be properly handled and be replaceable on every loop.
- Sounds that are disabled via purely a config are now not loaded - decreasing memory usage.
- Fixed issues that could happen if you really wanted to mess with `HideManagerGameObject`
- Updated README
- Fixed a log message accidently being put on LogDebug instead of LogExtended
- Fixed `update_every_frame` not correctly resetting.
- Cleared up messaging on deprecated `randomness`
and like some other internal changes that don't matter :3
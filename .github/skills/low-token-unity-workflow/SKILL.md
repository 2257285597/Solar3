---
name: low-token-unity-workflow
description: 'Use when working in Unity Solar3 and wanting minimal token usage, context-limit safety, module-first code edits, and fast targeted diagnostics. Trigger for: low token, token saving, minimal scan, context overflow, module lookup, quick patch, incremental verification.'
argument-hint: 'Task + target module (spawning/player/ui/visual/gameplay)'
user-invocable: true
disable-model-invocation: false
---

# Low Token Unity Workflow

## What This Skill Produces
- Minimal-context implementation workflow for Unity tasks.
- Module-first lookup and function-level reads only.
- Small patches with targeted validation.
- Concise status output that avoids repeated project-wide scans.

## When To Use
- User asks to save tokens or reduce context usage.
- Task is in Solar3 and maps to known modules.
- Need to continue work across new chats without re-reading the whole repo.

## Module Map
- Spawning: [spawning quick sheet](../../../docs/quick-lookup/QUICK_MODULE_SPAWNING.md)
- Player Motion: [player quick sheet](../../../docs/quick-lookup/QUICK_MODULE_PLAYER_MOTION.md)
- UI + Mutation: [ui/mutation quick sheet](../../../docs/quick-lookup/QUICK_MODULE_UI_MUTATION.md)
- Visual + Camera: [visual/camera quick sheet](../../../docs/quick-lookup/QUICK_MODULE_VISUAL_CAMERA.md)
- Gameplay Loop: [gameplay loop quick sheet](../../../docs/quick-lookup/QUICK_MODULE_GAMEPLAY_LOOP.md)
- Global index: [project quick lookup](../../../docs/quick-lookup/PROJECT_QUICK_LOOKUP.md)

## Procedure
1. Classify the user request into one primary module.
2. Read only the matched quick sheet first.
3. Read only required function-level ranges from 1-2 target files.
4. Apply the smallest viable patch.
5. Validate only edited files (errors/problems check).
6. Report results with short actionable notes.

## Decision Points
- If module is clear: read one module sheet only.
- If module is ambiguous: read global index, then choose one module.
- If issue spans multiple modules: start with primary module, then add one secondary module only.
- If compile errors appear outside changed files: report, do not start full repo sweep unless user asks.

## Completion Checks
- Uses module-first lookup before code scanning.
- Reads are limited to relevant ranges/functions.
- Patch scope is minimal and localized.
- Validation is limited to changed files.
- Response includes changed files and what to tune next (if applicable).

## Guardrails
- Avoid repeating unchanged plans and large summaries.
- Avoid whole-project grep/read unless explicitly requested.
- Prefer reusing existing architecture rather than adding logic into central orchestrators.

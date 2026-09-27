# Continuous Integration

CI (continuous integration) is an automatic check that runs on GitHub's servers every
time code is pushed to `main` or a pull request is opened against it. It answers one
question: *does the project still build and do all tests pass?*

## Where it is defined

[`.github/workflows/build.yml`](../.github/workflows/build.yml). GitHub picks up every
`.yml` file in `.github/workflows/` automatically — no setup on the website is needed.

## What it does

| Step | Meaning |
|---|---|
| `on: push / pull_request` | Run on every push to `main` and on every pull request targeting `main`. |
| `runs-on: windows-latest` | Use a fresh Windows virtual machine provided by GitHub (free for public repositories). |
| `actions/checkout` | Download the repository onto that machine. |
| `actions/setup-dotnet` | Install the .NET SDK version pinned in `global.json`. |
| `dotnet restore` | Download NuGet packages. |
| `dotnet build --configuration Release` | Compile everything. Warnings are errors, so any warning fails the build. |
| `dotnet test` | Run the test suite. |

## Where to see the result

- On GitHub, the **Actions** tab lists every run with its full log.
- Every commit and pull request gets a ✅ or ❌ next to it.
- A failing run on a pull request means it should not be merged until fixed.

## Running the same check locally

```bash
dotnet build --configuration Release
dotnet test --configuration Release
```

## Temporary setting

`tests/Pianista.Tests/Pianista.Tests.csproj` passes `--ignore-exit-code 8` so that a test
run with zero tests does not fail. Remove it once the first test is added.

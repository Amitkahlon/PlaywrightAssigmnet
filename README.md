# Playwright Automation Assignment

Automated UI and API tests for the [Playwright (software)](https://en.wikipedia.org/wiki/Playwright_(software)) Wikipedia page.

## Tech Stack

- C# / .NET 8
- Playwright for .NET
- NUnit 3

## Project Structure

```
Pages/          # Page Object Model
Services/       # API service layer
Tests/          # Test fixtures
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Playwright browsers installed:

```bash
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install
```

## Running the Tests

Run all tests (headed, with HTML report):

```bash
dotnet test
```

Run a specific task:

```bash
dotnet test --filter "FullyQualifiedName~DebuggingSectionTests"
dotnet test --filter "FullyQualifiedName~MicrosoftDevToolsTests"
dotnet test --filter "FullyQualifiedName~WikiColorTests"
```

The HTML report is generated at `TestResults/TestReport.html` after each run.

## Test Cases

### Task 1 — Unique Word Count (UI vs API)
Extracts the "Debugging features" section both via UI (POM) and via the MediaWiki Parse API, normalizes the text, counts unique words, and asserts the counts match.

### Task 2 — Microsoft Development Tools Links
Navigates to the Microsoft development tools navbox and validates that every technology name is a text link.

### Task 3 — Dark Color Theme
Opens the Color (beta) appearance panel and selects "Dark", then validates the `skin-theme-clientpref-night` CSS class is applied to the page.

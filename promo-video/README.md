# Approval System promo video (Remotion)

A ~43 second, 1080p promo video written in React. Free to run on your own machine.

## Setup (once)
1. Install Node.js 18+ from nodejs.org
2. In this folder run: `npm install`

## Preview and edit live
`npm start` opens Remotion Studio in your browser. Edit `src/Video.tsx` and the preview updates instantly.
Change the text in the `CONTENT`, `NODES`, `FEATURES` and `STACK` blocks at the top of the file.

## Render to MP4
Option 1: in Remotion Studio click **Render** (top right), keep MP4 (H.264), then Render.
Option 2: stop the preview (`Ctrl + C`) and run `npm run render`. The file appears at `out/approval-promo.mp4`.
The first render downloads a small copy of Chrome. This is normal.

## Storyboard and voiceover
| Scene | Time | On screen | Voiceover |
|---|---|---|---|
| 1 Hook | 0-4s | "Approvals lost in email chains? Route every request through the right people, in the right order." | "Approvals lost in email chains? Let's route every request through the right people, in the right order." |
| 2 Title | 4-9s | Project name, tagline, tech pill | "This is Approval System, a configurable multi-step approval workflow API built with ASP.NET Core." |
| 3 Flow | 9-22s | Requester -> Step 1 -> Step 2 -> Approved, example amount rule, audit caption | "A requester submits a request. The engine assigns the first approver by role, or by an amount rule, and notifies them. Each approval moves it to the next step until it's approved. Every action is audited." |
| 4 Features | 22-33s | Six feature cards | "Configurable workflows, rule-based routing, a full audit trail, in-app and email notifications, reports, and a JWT-secured REST API." |
| 5 Stack | 33-38s | Tech chips | "Built with .NET 10, Entity Framework Core, PostgreSQL, JWT and MailKit." |
| 6 Outro | 38-43s | Name, role, repo link | "I'm Collins, a backend engineer. The code is on GitHub, and I'm open to remote roles." |

## Add your voice
Record on your phone, export as `voice.mp3`, place it in a `public/` folder, then follow the comment at the top of `src/Video.tsx`. If your voice runs longer than 43s, change the numbers in the `D` object (30 frames = 1 second).

## Before you publish
The "Example rule" caption and the amounts are illustrative, not data from a real deployment. Edit them if you prefer other examples.

import React from "react";
import {
  AbsoluteFill,
  Sequence,
  interpolate,
  spring,
  useCurrentFrame,
  useVideoConfig,
} from "remotion";
// To add a voiceover later: put voice.mp3 in /public, then
// import { Audio, staticFile } from "remotion"; and render
// <Audio src={staticFile("voice.mp3")} /> inside ApprovalPromo.

// ---------- EDIT THESE ----------
const CONTENT = {
  product: "Approval System",
  tagline: "Configurable multi-step approval workflows",
  author: "Collins",
  role: "Backend Software Engineer",
  repo: "github.com/collarfil/ApprovalSystem",
  cta: "Open to remote backend roles",
};

const NODES = [
  { title: "Requester", sub: "submits a ₦2,500,000 request" },
  { title: "Step 1", sub: "Manager review" },
  { title: "Step 2", sub: "Finance review" },
  { title: "Approved", sub: "requester notified" },
];

const FEATURES = [
  { t: "Configurable workflows", d: "Ordered steps mapped to roles" },
  { t: "Rule-based routing", d: "Route approvers by request amount" },
  { t: "Full audit trail", d: "Every action logged and attributable" },
  { t: "Notifications", d: "In-app alerts plus SMTP email" },
  { t: "Reports", d: "Trends, status, approval times" },
  { t: "Secure REST API", d: "JWT auth with Swagger docs" },
];

const STACK = [".NET 10", "ASP.NET Core", "EF Core", "PostgreSQL", "JWT", "MailKit", "FluentValidation", "Swagger"];
// --------------------------------

const C = {
  bg: "#0b1220",
  panel: "#131c31",
  line: "#2a3858",
  text: "#e8edf7",
  muted: "#8ea0c4",
  accent: "#2dd4a4",
  blue: "#5b8cff",
};
const FONT =
  "Inter, -apple-system, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif";

// Scene lengths in frames (30 fps)
const D = { hook: 120, title: 150, flow: 390, features: 330, stack: 150, outro: 150 };
export const TOTAL_FRAMES = D.hook + D.title + D.flow + D.features + D.stack + D.outro;

const clamp = { extrapolateLeft: "clamp", extrapolateRight: "clamp" } as const;

const Scene: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const frame = useCurrentFrame();
  const { durationInFrames } = useVideoConfig();
  const opacity = interpolate(
    frame,
    [0, 10, durationInFrames - 10, durationInFrames],
    [0, 1, 1, 0],
    clamp
  );
  return (
    <AbsoluteFill style={{ background: C.bg, color: C.text, fontFamily: FONT, opacity }}>
      {children}
    </AbsoluteFill>
  );
};

const Pop: React.FC<{
  delay?: number;
  children: React.ReactNode;
  style?: React.CSSProperties;
}> = ({ delay = 0, children, style }) => {
  const frame = useCurrentFrame();
  const { fps } = useVideoConfig();
  const s = spring({ frame: frame - delay, fps, config: { damping: 200 } });
  return (
    <div style={{ opacity: s, transform: `translateY(${(1 - s) * 30}px)`, ...style }}>
      {children}
    </div>
  );
};

const Center: React.FC<{ children: React.ReactNode }> = ({ children }) => (
  <AbsoluteFill
    style={{ justifyContent: "center", alignItems: "center", textAlign: "center", padding: 120, gap: 28 }}
  >
    {children}
  </AbsoluteFill>
);

// 1. Hook
const Hook: React.FC = () => (
  <Scene>
    <Center>
      <Pop>
        <div style={{ fontSize: 76, fontWeight: 800, lineHeight: 1.15 }}>
          Approvals lost in email chains?
        </div>
      </Pop>
      <Pop delay={35}>
        <div style={{ fontSize: 66, fontWeight: 800, color: C.accent, lineHeight: 1.15 }}>
          Route every request through the right people, in the right order.
        </div>
      </Pop>
    </Center>
  </Scene>
);

// 2. Title
const Title: React.FC = () => (
  <Scene>
    <Center>
      <Pop>
        <div style={{ fontSize: 120, fontWeight: 900, letterSpacing: -2 }}>{CONTENT.product}</div>
      </Pop>
      <Pop delay={20}>
        <div style={{ fontSize: 44, color: C.muted }}>{CONTENT.tagline}</div>
      </Pop>
      <Pop delay={40}>
        <div
          style={{
            fontSize: 30,
            color: C.accent,
            border: `2px solid ${C.accent}`,
            borderRadius: 999,
            padding: "10px 28px",
          }}
        >
          ASP.NET Core · Clean Architecture · PostgreSQL
        </div>
      </Pop>
    </Center>
  </Scene>
);

// 3. Workflow flow (SVG, 1920x1080)
const W = 320;
const H = 150;
const GAP = 100;
const X0 = 170;
const Y = 400;
const nodeX = (i: number) => X0 + i * (W + GAP);

const Flow: React.FC = () => {
  const frame = useCurrentFrame();
  const appear = (d: number) => interpolate(frame, [d, d + 15], [0, 1], clamp);
  const segStart = (k: number) => 80 + k * 60;
  const segEnd = (k: number) => segStart(k) + 40;
  const cy = Y + H / 2;

  return (
    <Scene>
      <div style={{ position: "absolute", top: 90, width: "100%", textAlign: "center", fontSize: 58, fontWeight: 800 }}>
        Approvals, step by step
      </div>
      <svg viewBox="0 0 1920 1080" width={1920} height={1080} style={{ position: "absolute", inset: 0 }}>
        {/* connectors */}
        <g stroke={C.line} strokeWidth={4} opacity={appear(30)}>
          {[0, 1, 2].map((k) => (
            <line key={k} x1={nodeX(k) + W} y1={cy} x2={nodeX(k + 1)} y2={cy} />
          ))}
        </g>
        {/* nodes */}
        {NODES.map((n, i) => {
          const active = i === 0 || frame >= segEnd(i - 1);
          const isLast = i === NODES.length - 1;
          const stroke = active ? C.accent : C.line;
          return (
            <g key={n.title} opacity={appear(10 + i * 12)}>
              <rect x={nodeX(i)} y={Y} width={W} height={H} rx={20} fill={C.panel} stroke={stroke} strokeWidth={active ? 5 : 3} />
              <text x={nodeX(i) + W / 2} y={Y + 66} textAnchor="middle" fill={isLast && active ? C.accent : C.text} fontSize={40} fontWeight={800} fontFamily={FONT}>
                {n.title}
              </text>
              <text x={nodeX(i) + W / 2} y={Y + 108} textAnchor="middle" fill={C.muted} fontSize={26} fontFamily={FONT}>
                {n.sub}
              </text>
            </g>
          );
        })}
        {/* travelling dot */}
        {[0, 1, 2].map((k) => {
          if (frame < segStart(k) || frame > segEnd(k)) return null;
          const p = interpolate(frame, [segStart(k), segEnd(k)], [0, 1], clamp);
          const x = nodeX(k) + W + (nodeX(k + 1) - nodeX(k) - W) * p;
          return <circle key={k} cx={x} cy={cy} r={14} fill={C.accent} />;
        })}
      </svg>
      <div
        style={{
          position: "absolute",
          top: 680,
          width: "100%",
          textAlign: "center",
          fontSize: 38,
          color: C.blue,
          fontWeight: 700,
          opacity: interpolate(frame, [260, 285], [0, 1], clamp),
        }}
      >
        Example rule: Amount &gt; ₦1,000,000 routes to the Director role
      </div>
      <div
        style={{
          position: "absolute",
          bottom: 110,
          width: "100%",
          textAlign: "center",
          fontSize: 42,
          color: C.accent,
          fontWeight: 700,
          opacity: interpolate(frame, [310, 335], [0, 1], clamp),
        }}
      >
        Every action is audited and notified.
      </div>
    </Scene>
  );
};

// 4. Features
const Features: React.FC = () => (
  <Scene>
    <div style={{ position: "absolute", top: 80, width: "100%", textAlign: "center", fontSize: 60, fontWeight: 800 }}>
      What's inside
    </div>
    <AbsoluteFill
      style={{
        top: 220,
        padding: "0 120px",
        display: "grid",
        gridTemplateColumns: "1fr 1fr 1fr",
        gridAutoRows: 270,
        gap: 36,
        alignContent: "start",
      }}
    >
      {FEATURES.map((f, i) => (
        <Pop
          key={f.t}
          delay={12 + i * 18}
          style={{
            background: C.panel,
            border: `2px solid ${C.line}`,
            borderRadius: 24,
            padding: 36,
            display: "flex",
            flexDirection: "column",
            justifyContent: "center",
            gap: 12,
          }}
        >
          <div style={{ fontSize: 40, fontWeight: 800, color: C.accent }}>{f.t}</div>
          <div style={{ fontSize: 29, color: C.muted }}>{f.d}</div>
        </Pop>
      ))}
    </AbsoluteFill>
  </Scene>
);

// 5. Stack
const Stack: React.FC = () => (
  <Scene>
    <Center>
      <Pop>
        <div style={{ fontSize: 60, fontWeight: 800 }}>Built with</div>
      </Pop>
      <div style={{ display: "flex", flexWrap: "wrap", gap: 24, justifyContent: "center", maxWidth: 1500 }}>
        {STACK.map((s, i) => (
          <Pop
            key={s}
            delay={15 + i * 10}
            style={{
              fontSize: 40,
              fontWeight: 700,
              background: C.panel,
              border: `2px solid ${C.blue}`,
              borderRadius: 999,
              padding: "16px 36px",
            }}
          >
            {s}
          </Pop>
        ))}
      </div>
    </Center>
  </Scene>
);

// 6. Outro
const Outro: React.FC = () => (
  <Scene>
    <Center>
      <Pop>
        <div style={{ fontSize: 96, fontWeight: 900 }}>{CONTENT.author}</div>
      </Pop>
      <Pop delay={15}>
        <div style={{ fontSize: 42, color: C.muted }}>{CONTENT.role}</div>
      </Pop>
      <Pop delay={30}>
        <div style={{ fontSize: 44, color: C.accent, fontWeight: 700 }}>{CONTENT.cta}</div>
      </Pop>
      <Pop delay={45}>
        <div style={{ fontSize: 34, color: C.text }}>{CONTENT.repo}</div>
      </Pop>
    </Center>
  </Scene>
);

export const ApprovalPromo: React.FC = () => {
  let t = 0;
  const seq = (len: number) => {
    const from = t;
    t += len;
    return { from, durationInFrames: len };
  };
  return (
    <AbsoluteFill style={{ background: C.bg }}>
      <Sequence {...seq(D.hook)}><Hook /></Sequence>
      <Sequence {...seq(D.title)}><Title /></Sequence>
      <Sequence {...seq(D.flow)}><Flow /></Sequence>
      <Sequence {...seq(D.features)}><Features /></Sequence>
      <Sequence {...seq(D.stack)}><Stack /></Sequence>
      <Sequence {...seq(D.outro)}><Outro /></Sequence>
    </AbsoluteFill>
  );
};

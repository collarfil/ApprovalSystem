import React from "react";
import { Composition } from "remotion";
import { ApprovalPromo, TOTAL_FRAMES } from "./Video";

export const RemotionRoot: React.FC = () => (
  <Composition
    id="ApprovalPromo"
    component={ApprovalPromo}
    durationInFrames={TOTAL_FRAMES}
    fps={30}
    width={1920}
    height={1080}
  />
);

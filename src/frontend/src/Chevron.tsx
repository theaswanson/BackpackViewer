import { Property } from "csstype";

export const Chevron = ({ fill }: { fill?: Property.Fill }) => (
  <svg viewBox='0 0 100 50' height='17' xmlns='http://www.w3.org/2000/svg'>
    <polygon points='0,0 50,50 100, 0' style={{ fill: fill ?? "#ebe4ca" }} />
  </svg>
);

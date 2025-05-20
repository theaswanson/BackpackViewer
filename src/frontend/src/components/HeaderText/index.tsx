import { DetailedHTMLProps, HTMLAttributes, ReactNode } from "react";
import "./HeaderText.css";

type IProps = {
  title: string;
  titlePrefix?: ReactNode;
} & DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>;

export const HeaderText = ({
  title,
  titlePrefix,
  className,
  ...rest
}: IProps) => (
  <div
    {...rest}
    className={className ? `${className} header-text` : "header-text"}
  >
    {titlePrefix}
    <h1>{title}</h1>
  </div>
);

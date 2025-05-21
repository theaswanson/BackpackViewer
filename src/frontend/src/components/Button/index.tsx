import {
  ButtonHTMLAttributes,
  DetailedHTMLProps,
  PropsWithChildren,
} from "react";
import "./Button.css";

type IProps = DetailedHTMLProps<
  ButtonHTMLAttributes<HTMLButtonElement>,
  HTMLButtonElement
>;

export const Button = ({
  className,
  children,
  ...rest
}: PropsWithChildren<IProps>) => (
  <button {...rest} className={className ? `${className} button` : "button"}>
    {children}
  </button>
);

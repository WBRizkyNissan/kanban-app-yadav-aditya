export default function Button({
  as = 'button',
  className = '',
  children,
  ...props
}) {
  const Comp = as
  return (
    <Comp
      className={
        'inline-flex items-center justify-center rounded-md px-3 py-2 text-sm font-medium ' +
        'focus:outline-none focus:ring-2 focus:ring-blue-500 ' +
        className
      }
      {...props}
    >
      {children}
    </Comp>
  )
}

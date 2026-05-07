const Error = (prop: {ErrorMessage: string})=>{
  return <>
    <div class={"text-center"}>{prop.ErrorMessage}</div>
    </>
};

export default Error;
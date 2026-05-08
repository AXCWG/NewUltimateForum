import * as api from "./client/sdk.gen"
const UltimateForum = api;
const CutByEllipsis = (source: string|null | undefined) : string | null=> source ?  source.length >= 10 ? source?.substring(0, 11) + "..." : source??null : null
export {UltimateForum, CutByEllipsis}
class UltimateForum{
	private _apiAddr: string;
	constructor(apiAddr: string) {
		this._apiAddr = apiAddr;
	}
	public async Login(loginInfo: LoginInformation){
		const login = await fetch(this._apiAddr, {
			method: 'POST', body: JSON.stringify(loginInfo), credentials: 'include', headers: {'Content-Type': 'application/json'}
		});
		if(login.ok){
			return true;
		}
		console.log(`login failed: ${await login.text()}`)
		return false;
	}
}
interface LoginInformation{
	Username:string;
	Password:string | null;
}
export type {LoginInformation}; 
export default UltimateForum;
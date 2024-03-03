import InventoryTypeApi, {
	type CreateInventoryTypeRequest,
	type EditInventoryTypeRequest,
	type InventoryTypeResponse
} from '$lib/api/inventoryTypeApi';
import { SiteMap } from '$lib/utils/siteMap';
import type { ApiResponse } from '$lib/api/baseApi';

export default class InventoryTypeService {
	errors: string[] = [];
	api: InventoryTypeApi;

	constructor(
		private gameId: string,
		private navigate: (path: string) => void
	) {
		this.api = new InventoryTypeApi();
	}

	async CreateInventoryType(request: CreateInventoryTypeRequest) {
		let result = await this.api.CreateInventoryType(this.gameId, request);

		if (result.errors) {
			this.handleErrorResponse(result);
		} else this.navigateToGameHome();
	}

	private handleErrorResponse(result: ApiResponse<InventoryTypeResponse>) {
		this.errors = result.errors;
		return;
	}

	private navigateToGameHome() {
		this.navigate(SiteMap.game.home(this.gameId));
	}

	async EditInventoryType(request: EditInventoryTypeRequest) {
		let result = await this.api.EditInventoryType(request.inventoryTypeId, request);

		if (result.errors) this.handleErrorResponse(result);
		else this.navigateToGameHome();
	}

	async GetInventoryType(id: string): Promise<InventoryTypeResponse> {
		let result = await this.api.GetInventoryTypeById(id);

		if (result.errors) this.handleErrorResponse(result);

		return result?.response ?? null;
	}
}

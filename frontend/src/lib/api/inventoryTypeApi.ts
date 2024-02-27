import BaseApi, { type ApiResponse } from '$lib/api/baseApi';

export interface InventoryTypeResponse {
	gameId: string;
	inventoryTypeId: string;
	name: string;
	options: InventoryTypeOption[];
}

export interface InventoryTypeOption {
	label: string;
	value: string;
}

export interface CreateInventoryTypeRequest {
	name: string;
	options: InventoryTypeOption[];
}

export interface EditInventoryTypeRequest extends CreateInventoryTypeRequest {
	inventoryTypeId: string;
}

export default class InventoryTypeApi extends BaseApi {
	async GetInventoryTypeById(id: string): Promise<ApiResponse<InventoryTypeResponse>> {
		return await this.Get(`InventoryTypes/${id}`);
	}

	async CreateInventoryType(
		gameId: string,
		request: CreateInventoryTypeRequest
	): Promise<ApiResponse<InventoryTypeResponse>> {
		return await this.Post(`InventoryTypes/Create/${gameId}`, request);
	}

	async EditInventoryType(
		inventoryTypeId: string,
		request: EditInventoryTypeRequest
	): Promise<ApiResponse<InventoryTypeResponse>> {
		return await this.Put(`InventoryTypes/Edit/${inventoryTypeId}`, request);
	}

	async GetInventoryTypesForGame(gameId: string): Promise<ApiResponse<InventoryTypeResponse[]>> {
		return await this.Get(`InventoryTypes/ForGame/${gameId}`);
	}
}

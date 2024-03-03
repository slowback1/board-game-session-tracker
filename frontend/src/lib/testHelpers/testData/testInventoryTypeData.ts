import type {
	CreateInventoryTypeRequest,
	EditInventoryTypeRequest,
	InventoryTypeOption,
	InventoryTypeResponse
} from '$lib/api/inventoryTypeApi';

export const testInventoryTypeOption: InventoryTypeOption = {
	label: 'label',
	value: 'value'
};

export const testInventoryTypeResponse: InventoryTypeResponse = {
	inventoryTypeId: '1234',
	options: [testInventoryTypeOption, testInventoryTypeOption],
	name: 'test name',
	gameId: 'a1-2b'
};

export const testCreateInventoryTypeRequest: CreateInventoryTypeRequest = {
	name: 'name',
	options: [testInventoryTypeOption]
};

export const testEditInventoryTypeRequest: EditInventoryTypeRequest = {
	options: [testInventoryTypeOption],
	name: 'edited name',
	inventoryTypeId: '1234'
};

import { beforeEach, describe } from 'vitest';
import ToastService, { type ToastConfig, ToastType } from './ToastService';
import MessageBus from '../bus/MessageBus';
import { Messages } from '../bus/Messages';

describe('ToastService', () => {
	function getCurrentToastData(): ToastConfig[] {
		return MessageBus.getLastMessage(Messages.ToastData);
	}

	beforeEach(() => {
		ToastService.initialize();
	});

	describe('initializing', () => {
		it('sets the message bus config to not store toasts between page loads', () => {
			expect(MessageBus.configure.shouldStoreDataForMessage(Messages.ToastData)).toEqual(false);
		});

		it('sets the toast message to an empty array', () => {
			let toasts = getCurrentToastData();

			expect(toasts).toEqual([]);
		});
	});

	describe('actions', () => {
		let service: ToastService;

		beforeEach(() => {
			service = new ToastService();
		});

		it('can add a toast', () => {
			service.addToast({ message: 'hello', type: ToastType.success });

			let toasts = getCurrentToastData();

			expect(toasts.length).toEqual(1);
		});
	});
});

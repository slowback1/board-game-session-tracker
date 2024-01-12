import MessageBus from '../bus/MessageBus';
import { Messages } from '../bus/Messages';

export enum ToastType {
	success,
	warning,
	error
}

export type ToastConfig = {
	message: string;
	type: ToastType;
};

export default class ToastService {
	static initialize() {
		MessageBus.configure.doNotStoreDataForMessage(Messages.ToastData);
		MessageBus.sendMessage(Messages.ToastData, []);
	}

	addToast(config: ToastConfig) {}
}

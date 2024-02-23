import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import { testUserResponse } from '$lib/testHelpers/testData/testUserData';

export function logUserInWithTestData() {
	MessageBus.sendMessage(Messages.UserToken, 'token');
	MessageBus.sendMessage(Messages.UserData, testUserResponse);
}

[![Build Status][travisimg]][travisurl]
[![Issues][issuesimg]][issuesurl]

# lizardFtp
Lizard is a work-in-progress FTP client and library, written in C# 5.

## Features: lizardcli
- Familiar wget-like command-line interface
- Upload/download files easily

## Features: libLizard
- Familiar TcpClient-like interface
- Event-driven for maximum efficiency
- Allows sending raw ftp commands
- Support for passive transfer
- Full RFC 2389 (FEAT) implementation

## ToDo: libLizard
- Add support for active transfer
- Add support for FTP over SSL
- Add full RFC 959 support

[issuesurl]: https://github.com/codeaddicts/lizardftp/issues
[issuesimg]: https://img.shields.io/github/issues/codeaddicts/lizardftp.svg?style=flat-square
[travisurl]: https://travis-ci.org/codeaddicts/lizardftp
[travisimg]: https://img.shields.io/travis/codeaddicts/lizardftp/master.svg?style=flat-square

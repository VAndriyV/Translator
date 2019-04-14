import React, { Component } from "react";
import { Link } from "react-router-dom";
import { Glyphicon, Nav, Navbar, NavItem } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import "./NavMenu.css";

export class NavMenu extends Component {
    displayName = NavMenu.name;

    render() {
        return (
            <Navbar inverse fixedTop fluid collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        <Link to={"/"}> <Glyphicon glyph="qrcode" /> Translator</Link>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                        <LinkContainer to={"/lexicalAnalyzer"} exact>
                            <NavItem>
                                <Glyphicon glyph="hdd" /> Lexical analyzer
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/recursiveAnalyzer"}>
                            <NavItem>
                                <Glyphicon glyph="sort-by-attributes-alt" /> Recursive syntax
                                analyzer
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/automaticAnalyzer"}>
                            <NavItem>
                                <Glyphicon glyph="link" /> Automatic syntax analyzer
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/relationsTable"}>
                            <NavItem>
                                <Glyphicon glyph="th" /> Relations table
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/ascendingAnalyzer"}>
                            <NavItem>
                                <Glyphicon glyph="arrow-up" /> Ascending syntax analyzer
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/rpnExpression"}>
                            <NavItem>
                                <Glyphicon glyph="console" /> Reverse Polish notation (expression only)
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/rpnBuilder"} exact>
                            <NavItem>
                                <Glyphicon glyph="retweet" /> Reverse Polish notation
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/executeProgram"} exact>
                            <NavItem>
                                <Glyphicon glyph="cd" /> Execute program
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/performHashing"} exact>
                            <NavItem>
                                <Glyphicon glyph="barcode" /> Hashing
              </NavItem>
                        </LinkContainer>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

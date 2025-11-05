<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="Comparision">
				<xsl:if test ="substring(COL1,1,16)!='6320100329POSTNV' and (substring(COL1,1,8)='19210400' or substring(COL1,1,8)='19210327')">
					
				
				<PositionMaster>
					<xsl:variable name = "PB_FUND_NAME" >
					
				<xsl:value-of select="substring(COL1,1,7)"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select="''"/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</AccountName>
						</xsl:otherwise>
					</xsl:choose>

					<!--side-->
					<xsl:choose>
						<xsl:when test="substring(COL1,81,1)='+'">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
						
						</xsl:when>
						<xsl:when test="substring(COL1,81,1)='-'">
							<Side>
								<xsl:value-of select="'Sell'"/>
							</Side>
						</xsl:when>
						<xsl:otherwise>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
						</xsl:otherwise>
					</xsl:choose>
						    <Quantity>
								<xsl:value-of select="format-number(substring(COL1,64,17),'#.####')"/>
							</Quantity>
					<MarketValue>
						<xsl:value-of select="format-number(substring(COL1,84,16),'#.####')"/>
					</MarketValue>
					
					<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,48,15))"/>
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
					</xsl:variable>

					<xsl:variable name ="varSymbol">
						<xsl:value-of select ="normalize-space(substring(COL1,25,6))"/>
					</xsl:variable>
					<xsl:variable name ="varSymboloption">
						<xsl:value-of select ="concat(substring(COL1,114,6),substring(COL1,123,6),substring(COL1,152,1),substring(COL1,139,5),substring(COL1,145,3))"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME != ''">
							<Symbol>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:when>
						<xsl:when test ="normalize-space($varSymboloption)=''">
							<Symbol>
								<xsl:value-of select="$varSymbol"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<IDCOOptionSymbol>
								<xsl:value-of select="concat($varSymboloption,'U')"/>
							</IDCOOptionSymbol>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME != ''">
							<PBSymbol>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</PBSymbol>
						</xsl:when>
						<xsl:when test ="normalize-space($varSymboloption)=''">
							<PBSymbol>
								<xsl:value-of select="$varSymbol"/>
							</PBSymbol>
							</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="concat($varSymboloption,'U')"/>
							</PBSymbol>
							</xsl:otherwise>
					</xsl:choose >
					<xsl:variable name ="varMarkPrice">
						<xsl:value-of select ="format-number(substring(COL1,101,13),'#.####')"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="boolean(number($varMarkPrice))">
							<MarkPrice>
								<xsl:value-of select="$varMarkPrice"/>
							</MarkPrice>
						</xsl:when>
						<xsl:otherwise>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
						</xsl:otherwise>
					    </xsl:choose>

					<SMRequest>
						<xsl:value-of select ="'TRUE'"/>
					</SMRequest>


				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

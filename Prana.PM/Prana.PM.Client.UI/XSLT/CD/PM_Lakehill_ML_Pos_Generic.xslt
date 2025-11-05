<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=101">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=102">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=103">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=104">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=105">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=106">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=107">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=108">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=109">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=110">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=111">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=112">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<!-- Put month Codes e.g. 001 represents 0=Put, 01 = January-->
			<xsl:when test ="$varMonth=001">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=002">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=003">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=004">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=005">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=006">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=007">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=008">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=009">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=010">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=011">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=012">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="COL1!='side' and COL1!='M' and COL2 !='Symbol'">
					<PositionMaster>
						<!--   Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL6"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="''"/>
							<!--<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>-->
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='""'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="''"/>
							<!--<xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name='ML']/StrategyData[@PranaFund=$PB_FUND_NAME]/@PranaStrategy"/>-->
						</xsl:variable>


						<xsl:choose>
							<xsl:when test="$PRANA_STRATEGY_NAME=''">
								<Strategy>
									<xsl:value-of select="''"/>
								</Strategy>
							</xsl:when>
							<xsl:otherwise>
								<Strategy>
									<xsl:value-of select='$PRANA_STRATEGY_NAME'/>
								</Strategy>
							</xsl:otherwise>
						</xsl:choose >
						
						<xsl:variable name="varPBSymbol" select="COL2"/>
						
						<xsl:variable name="PB_COMPANY_NAME" select="COL2"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="''"/> 
								<!--select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>-->
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="$varPBSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>
						<!--<IDCOOptionSymbol>
							<xsl:value-of select="''"/>
						</IDCOOptionSymbol>-->

						<PBSymbol>
							<xsl:value-of select ="concat (COL2, '   Side:', COL7, ' Strategy: ', $PRANA_STRATEGY_NAME)"/>
						</PBSymbol>


						<xsl:choose>
							<xsl:when test="COL3 &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL3"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL3 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL3*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>


					

						<xsl:choose>
							<xsl:when test ="normalize-space(COL7) ='Buy'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when >
							<xsl:when test ="normalize-space(COL7)='Sell short'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when >
							<xsl:when test ="normalize-space(COL7)='Sell to Open'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when >
							<xsl:when test ="normalize-space(COL7)='Buy to Open'">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when >
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:choose>
							<xsl:when test ="COL10 = 'tradedate' or COL10='*'">
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="COL10"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varNotional" select="COL9"/>

						<xsl:variable name ="varAssetClass" select ="normalize-space(COL8)"/>


						<xsl:choose>
							<xsl:when test="boolean(number(COL9)) and $varAssetClass = 'EquityOption' ">
								<CostBasis>
									<xsl:value-of select="(COL9) div (100*COL3)"/>
								</CostBasis>
							</xsl:when>
							<xsl:when test="boolean(number(COL4)) ">
								<CostBasis>
									<xsl:value-of select="COL4"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name="PB_CounterParty" select="normalize-space(COL6)"/>
						<xsl:variable name="PRANA_CounterPartyCode">
							<xsl:value-of select="''"/>
							<!--<xsl:value-of select="document('../ReconMappingXml/FundWisePBMapping.xml')/BrokerMapping/PB[@Name='ML']/BrokerData[@PranaFundName =$PB_CounterParty]/@PB"/>-->
						</xsl:variable>

						<!--<xsl:choose>
							<xsl:when test ="$PRANA_CounterPartyCode !=''">
								<ExecutingBroker>
									<xsl:value-of select="$PRANA_CounterPartyCode"/>
								</ExecutingBroker>
							</xsl:when>
							<xsl:otherwise>
								<ExecutingBroker>
									<xsl:value-of select="''"/>
								</ExecutingBroker>
							</xsl:otherwise>
						</xsl:choose>-->
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
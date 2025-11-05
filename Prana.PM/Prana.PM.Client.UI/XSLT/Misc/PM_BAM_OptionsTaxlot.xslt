<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='EQUITYOPTION'">				
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL12,'&quot;','')"/>
						</xsl:variable>
						<xsl:if test="COL12='BAM OPPORTUNITY FUND LP'">
							<FundName>
								<xsl:value-of select="'OFFSHORE'"/>
							</FundName>
						</xsl:if>					

						<!--  Symbol Region -->

						<!--<Symbol>
							<xsl:value-of select="Specify Column Name"/>
						</Symbol>-->

						<!-- Symbol Region Ends -->

						<!--   CUSIP -->
						<!-- Column 2 mapped with CUSIP-->						

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL8,'&quot;','')"/>
						</PBSymbol>

						<PBAssetType>
							<xsl:value-of select="COL2"/>
						</PBAssetType>
						
						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:if test="COL3 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL3*(-1)"/>
							</NetPosition>
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:if>
						<xsl:if test="COL3 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:if>

						<!-- Column 4 mapped with Side-->
						<!--<xsl:variable name = "varSide" >
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</xsl:variable>
						<xsl:if test="$varSide='L'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
							--><!--<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>--><!--
						</xsl:if>
						<xsl:if test="$varSide='S'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>-->
							<!--<Side>
								<xsl:value-of select="'Sell short'"/>
							</Side>-->
						<!--</xsl:if>-->

						

						<!--<xsl:if test="$varInstrumentType='FXForward'">
							<xsl:variable name = "varSymbol" >
								<xsl:value-of select="substring(COL20,0,11)"/>
							</xsl:variable>
							<Symbol>
								<xsl:value-of select="translate(translate($varSymbol, ' ' , ''),'TO','-')"/>
							</Symbol>
						</xsl:if>-->
						<!--  Average Price Region -->
						<!--<xsl:if test="$varInstrumentType='EquityOptions' or $varInstrumentType='CommonStock' or $varInstrumentType='DepositoryReceipts'">-->
							<CostBasis>
								<xsl:value-of select="COL10"/>
							</CostBasis>
						<!--</xsl:if>-->
						<!--<xsl:if test="$varInstrumentType='FXForward'">
							<xsl:variable name = "varMDIndicator" >
								<xsl:value-of select="translate(COL23, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
							</xsl:variable>
							<xsl:if test="$varMDIndicator='M'">
								<CostBasis>
									<xsl:value-of select="COL22"/>
								</CostBasis>
							</xsl:if>
							<xsl:if test="$varMDIndicator='D'">
								<CostBasis>
									<xsl:value-of select="1 div COL22"/>
								</CostBasis>
							</xsl:if>
						</xsl:if>-->

						<!--<CostBasis>
							<xsl:value-of select="Specify Column Name"/>
						</CostBasis>-->

						<!-- Symbol Region  -->

						<!--<SEDOL>
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</SEDOL>
						<ISIN>
							<xsl:value-of select="translate(COL38,'&quot;','')"/>
						</ISIN>

						<RIC>
							<xsl:value-of select="translate(COL46,'&quot;','')"/>
						</RIC>-->						
						<!-- Position Date mapped with the column 6 -->
						<xsl:variable name = "varDate" >
							<xsl:value-of select="substring-before(COL13,':')"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="$varDate"/>
						</PositionStartDate>
						
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length(COL8)"/>
							</xsl:variable>
							<xsl:variable name = "varStartIndex" >
								<xsl:value-of select="($varLength)-1"/>
							</xsl:variable>
						<xsl:variable name = "varAfter" >
							<xsl:value-of select="substring(COL8,$varStartIndex,2)"/>
						</xsl:variable>
						<xsl:variable name = "varBefore" >
							<xsl:value-of select="substring(COL8,1,($varStartIndex)-1)"/>
						</xsl:variable>
						<Symbol>
							<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
						</Symbol>
							<!--<Symbol>
								<xsl:value-of select="translate(concat($varBefSym,' ',$varAftSym),'&quot;','')"/>
							</Symbol>-->
						
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

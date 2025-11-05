<?xml version="1.0" encoding="utf-8"?>
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
					<xsl:value-of select="translate(COL7,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varImportTrueFalse" >
					<xsl:value-of select="translate(COL18,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varImportTrueFalse='Y'">
					<xsl:if test="$varInstrumentType='Equity' or $varInstrumentType='Options' or $varInstrumentType='CFD' ">
						<PositionMaster>
							<!--   Fund -->
							<!-- Column 17 (empty column) mapped with Fund hence fund always fixed-->
							<xsl:variable name = "varPortfolioID" >
								<xsl:value-of select="translate(COL18,'&quot;','')"/>
							</xsl:variable>
							
							<xsl:choose>
								<xsl:when test="$varPortfolioID='LUCAS TOP'">
									<FundName>
										<xsl:value-of select="'Letrol.002217487'"/>
									</FundName>
								</xsl:when>
								<xsl:when test="$varPortfolioID='000000005296005'">
									<FundName>
										<xsl:value-of select="'Letrol.002217487'"/>
									</FundName>
								</xsl:when>
								<xsl:otherwise>
									<FundName>
										<xsl:value-of select="'Letrol.002217487'"/>
									</FundName>
								</xsl:otherwise >
							</xsl:choose >	
							
							<PBAssetType>
								<xsl:value-of select="translate(COL7,'&quot;','')"/>
							</PBAssetType>
							
							<Commissions>
								<xsl:value-of select="COL19"/>
							</Commissions>

							<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
							<xsl:choose>
								<xsl:when test ="COL4 &lt; 0">
									<NetPosition>
										<!--<xsl:value-of select='format-number(COL17*(-1),"###")'/>-->
										<xsl:value-of select="COL4*(-1)"/>
									</NetPosition>
								</xsl:when>
								<xsl:when test ="COL4 &gt; 0">
									<NetPosition>
										<!--<xsl:value-of select='format-number(COL17*(-1),"###")'/>-->
										<xsl:value-of select="COL4"/>
									</NetPosition>
								</xsl:when>
								<xsl:otherwise>
									<NetPosition>
										<xsl:value-of select="''"/>
									</NetPosition>
								</xsl:otherwise>
							</xsl:choose>

							<xsl:choose>
								<xsl:when test ="COL11 &lt; 0 or COL11 &gt; 0 or COL11=0 ">
									<CostBasis>
										<xsl:value-of select="COL11"/>
									</CostBasis>
								</xsl:when>
								<xsl:otherwise>
									<CostBasis>
										<xsl:value-of select="0"/>
									</CostBasis>
								</xsl:otherwise>
							</xsl:choose>
							
							<!-- Position Date mapped with the column 16 -->
							<PositionStartDate>
								<xsl:value-of select="translate(COL1,'&quot;','')"/>
							</PositionStartDate>

							<xsl:variable name = "varOrderSide" >
								<xsl:value-of select="translate(COL3,'&quot;','')"/>
							</xsl:variable>
							<!-- Prime Broker Symbol -->
							<xsl:if test="$varInstrumentType='Equity' or $varInstrumentType='CFD' ">
								<Symbol>
									<xsl:value-of select="translate(COL6,'&quot;','')"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
								
								<xsl:choose>
									<xsl:when test="$varOrderSide='BUY'">
										<SideTagValue>
											<xsl:value-of select="'1'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SELL'">
										<SideTagValue>
											<xsl:value-of select="'2'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='BUY CLOSE'">
										<SideTagValue>
											<xsl:value-of select="'B'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SHORT SELL'">
										<SideTagValue>
											<xsl:value-of select="'5'"/>
										</SideTagValue>
									</xsl:when>

								</xsl:choose >
							</xsl:if >
							
							<xsl:if test="$varInstrumentType='Options'">


								<xsl:choose>
									<xsl:when test="$varOrderSide='BUY'">
										<SideTagValue>
											<xsl:value-of select="'A'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SELL'">
										<SideTagValue>
											<xsl:value-of select="'D'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='BUY CLOSE'">
										<SideTagValue>
											<xsl:value-of select="'B'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SHORT SELL'">
										<SideTagValue>
											<xsl:value-of select="'C'"/>
										</SideTagValue>
									</xsl:when>

								</xsl:choose >
								<!-- $PXPHO-->
								<xsl:variable name="varAfterDollar" >
									<xsl:value-of select="translate(COL6,'&quot;','')"/>
								</xsl:variable>
								
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterDollar)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterDollar,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterDollar,1,($varLength)-2)"/>
								</xsl:variable>
								
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
								
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
							</xsl:if >
						</PositionMaster>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
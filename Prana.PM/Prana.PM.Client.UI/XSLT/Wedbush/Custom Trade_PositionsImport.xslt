<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL4)">

					<PositionMaster>
						
						<Symbol>
							<xsl:value-of select="COL3"/>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select="COL1"/>
						</PBSymbol>
						

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='FIXTrade']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:choose>
							<xsl:when test ="COL4 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL4 *(-1)"/>
								</NetPosition>
							</xsl:when >
							<xsl:when test ="COL4 &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL4"/>
								</NetPosition>
							</xsl:when >
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose >


						<xsl:choose>

							<xsl:when test ="(COL4 &gt; 0)">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when >


							<xsl:when test ="(COL4 &lt; 0)">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when >

							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose >

						<TransactionType>

							<!--<xsl:choose>

								<xsl:when test ="COL5='Long'">

									<xsl:value-of select="'LongAddition'"/>

								</xsl:when >
								<xsl:when test ="COL5='Short'">
									<xsl:value-of select="'ShortAddition'"/>
								</xsl:when >

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>-->

							<!--<xsl:choose>						

								<xsl:when test ="COL5='Long'">

									<xsl:value-of select="'LongWithdrawal'"/>

								</xsl:when >
								
								<xsl:when test ="COL5='Short'">
									<xsl:value-of select="'ShortWithdrawal'"/>
								</xsl:when >

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>-->

						</TransactionType>



						<xsl:choose>
							<xsl:when test ="boolean(number(COL4))">
								<CostBasis>
									<xsl:value-of select="COL12"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						          <CounterPartyID>							
									<xsl:value-of select="25"/>
								</CounterPartyID>

						<PositionStartDate>
							<xsl:value-of select="COL10"/>
						</PositionStartDate>

						<!--<OriginalPurchaseDate>
							<xsl:value-of select="COL2"/>
						</OriginalPurchaseDate>-->

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

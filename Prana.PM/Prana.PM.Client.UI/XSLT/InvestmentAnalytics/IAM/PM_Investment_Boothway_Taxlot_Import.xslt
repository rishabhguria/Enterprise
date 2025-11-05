<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<PositionMaster>

					<!--<FundName>
            <xsl:value-of select="''"/>
          </FundName>-->

            		<Symbol>
						<xsl:value-of select="COL6"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL7"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL222"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='FIXTrade']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>




					<xsl:choose>
						<xsl:when test ="COL17 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL17*(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL17 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL17"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >


					<xsl:choose>
						<!--<xsl:when test ="COL5 &gt; 0 ">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >-->
						<xsl:when test ="COL4='Long' ">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='Short' ">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='PurchaseShort' ">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='Purchase' ">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<!--<xsl:when test ="COL5 &lt; 0 ">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >-->
						
						<!--<xsl:when test ="COL7='Buy to Close'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="(COL7='Sell short' or COL7='Sell Short') and COL7= 'Equity'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Open'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Buy to Open'">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Close'">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when >-->

						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name="Cost">
						<xsl:choose>
							<xsl:when test="number(COL17)">
								<xsl:value-of select="COL20"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>



					<xsl:choose>
						<xsl:when test ="boolean(number($Cost))">
							<CostBasis>
								<xsl:value-of select="$Cost"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="boolean(number(COL1111))">
							<Commission>
								<xsl:value-of select="COL1111"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL5))">
							<MiscFees>
								<xsl:value-of select="COL5"/>
							</MiscFees>
						</xsl:when>
						<xsl:otherwise>
							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>
						</xsl:otherwise>
					</xsl:choose>-->

					<xsl:choose>
						<xsl:when test="boolean(number(COL177))">
							<Fees>
								<xsl:value-of select="COL177"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="normalize-space(COL19)"/>
					</xsl:variable>

					<xsl:variable name="PRANA_BROKER_ID">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='CON']/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>

					<CounterPartyID>
						<xsl:choose>
							<xsl:when test="number($PRANA_BROKER_ID)">
								<xsl:value-of select="$PRANA_BROKER_ID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CounterPartyID>


					<FXRate>
						<xsl:value-of select="COL22"/>
					</FXRate>


					<!--<xsl:variable name="FXRate">
						<xsl:choose>
							<xsl:when test="number(COL18)">
								<xsl:value-of select="COL18"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="boolean(number($FXRate))">
							<FXRate>
								<xsl:value-of select="$FXRate"/>
							</FXRate>
						</xsl:when>
						<xsl:otherwise>
							<FXRate>
								<xsl:value-of select="0"/>
							</FXRate>
						</xsl:otherwise>
					</xsl:choose>-->


					<PositionStartDate>
						<xsl:value-of select="COL13"/>
					</PositionStartDate>

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>

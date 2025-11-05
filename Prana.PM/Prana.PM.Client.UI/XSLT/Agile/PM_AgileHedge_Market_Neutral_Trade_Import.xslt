<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL12)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Agile'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL10='OPTN'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="COL10='OPTN'">
									<xsl:value-of select="concat(COL5,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select='$PB_FUND_NAME'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varDate">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>



						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL12"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL10='OPTN'">
									<xsl:choose>
										<xsl:when test ="$varSide='BY'">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:when test ="$varSide='SL'">
											<xsl:value-of select ="'D'"/>
										</xsl:when>
										<xsl:when test ="$varSide='CS'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="$varSide='SS'">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$varSide='BY'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										<xsl:when test ="$varSide='SL'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="$varSide='CS'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="$varSide='SS'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>

						</SideTagValue>

						<xsl:variable name="Commission">
							<xsl:value-of select="COL18"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ='$Commission*-1'/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>


						<xsl:variable name="StampDuty">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($StampDuty) &lt; 0'>
									<xsl:value-of select ='$StampDuty*-1'/>
								</xsl:when>
								<xsl:when test ='number($StampDuty) &gt; 0'>
									<xsl:value-of select ='$StampDuty'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>


						<xsl:variable name="MiscFees">
							<xsl:value-of select="number(COL20)-number(COL21)"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($MiscFees) &lt; 0'>
									<xsl:value-of select ='$MiscFees*-1'/>
								</xsl:when>
								<xsl:when test ='number($MiscFees) &gt; 0'>
									<xsl:value-of select ='$MiscFees'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name = "PB_BROKER_NAME" >
							<xsl:value-of select ="COL15"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_BROKER_ID!=''">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</CounterPartyID>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

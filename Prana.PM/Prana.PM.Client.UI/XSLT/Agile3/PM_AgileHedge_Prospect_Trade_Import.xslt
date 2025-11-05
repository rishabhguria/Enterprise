<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				
				<xsl:variable name="varQuantity" select="number(translate(COL4,',',''))"/>
				
				<xsl:if test="number($varQuantity)">
					
					<PositionMaster>					
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_CountnerParty" select="normalize-space(COL20)"/>

						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= $PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<xsl:variable name="AssetType">
							<xsl:choose>

								<xsl:when test="contains(COL6,'CALL')">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
							

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varEquitySymbol">
							<xsl:choose>
								<xsl:when test="$AssetType='Equity'">
									<xsl:value-of select="normalize-space(COL5)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<!--<xsl:variale name="Symbol" select="normalize-space(COL5)"/>-->

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$AssetType='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="COL6!='*'">
									<xsl:value-of select="$varEquitySymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$AssetType='Option'">
									<xsl:value-of select="concat(COL5,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
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

						<PositionStartDate>
							<xsl:value-of select="COL2"/>
						</PositionStartDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL2"/>
						</OriginalPurchaseDate>


						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CounterPartyID)">
									<xsl:value-of select="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<xsl:variable name="varSide" select="COL3"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BTC'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SEL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='SSL'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='STO'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='STC'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="translate($varSide, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BTO'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

					

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="translate(COL8,',','')"/>
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

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(translate(COL10,',',''),'(','')"/>
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

						<xsl:variable name="Fees">
							<xsl:value-of select="translate(COL11,',','')"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($Fees) &lt; 0'>
									<xsl:value-of select ='$Fees*-1'/>
								</xsl:when>
								<xsl:when test ='number($Fees) &gt; 0'>
									<xsl:value-of select ='$Fees'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="StampDuty">
							<xsl:value-of select="translate(COL10,',','')"/>
						</xsl:variable>

						<!--<StampDuty>
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
						</StampDuty>-->

						<!--<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL11)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JPM']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "COL5"/>
						</xsl:variable>


						<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(COL5, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_SymbolName">
							<xsl:value-of select="substring-before(COL5,' ')"/>
						</xsl:variable>
						<xsl:variable name="PB_Symbol" select="COL5"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:value-of select="15"/>
						</CounterPartyID>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1='DGL'">
									<xsl:value-of select="concat(translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.'),'/SWAP')"/>
								</xsl:when>
								<!--<xsl:otherwise>
									<xsl:choose>
										<xsl:when test=" $PB_ExchangeCODE='US' ">
											<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
										</xsl:when>-->
										<xsl:otherwise>
											<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
										</xsl:otherwise>
									<!--</xsl:choose>
								</xsl:otherwise>-->
							</xsl:choose>
						</Symbol>
						
						
						<!--<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "COL5"/>
						</xsl:variable>

						<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(COL5, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>



						<xsl:variable name="PB_SymbolName">
							<xsl:value-of select="substring-before(COL5,' ')"/>
						</xsl:variable>
						<xsl:variable name="PB_Symbol" select="COL5"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="$PRANA_Exchange = ''">
									<xsl:value-of select ="COL5"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						-->

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL11)) &gt; 0">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:when test="number(normalize-space(COL11)) &lt; 0">
									<xsl:value-of select="COL11* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<CostBasis>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL14))">
									<xsl:value-of select="COL14"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL12 = 'L'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL12 = 'S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL10"/>
						</PBSymbol>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="translate(concat($PRANA_SYMBOL_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
								</xsl:when>
								<!--<xsl:otherwise>
									<xsl:choose>
										<xsl:when test=" $PB_ExchangeCODE='US' ">
											<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
										</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="translate(concat(translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.'),'_swap'),$upper_CONST,$lower_CONST)"/>
								</xsl:otherwise>
								<!--</xsl:choose>
								</xsl:otherwise>-->
							</xsl:choose>
						</TradeAttribute1>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

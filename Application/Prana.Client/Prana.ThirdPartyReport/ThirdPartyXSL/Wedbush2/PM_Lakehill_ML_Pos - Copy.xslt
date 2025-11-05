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

				<!--<xsl:if test ="$varCALLPUT='EQUITY' or $varCALLPUT='OPT'">-->
					<PositionMaster>
						<!--   Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name ="varCALLPUT">
							<xsl:value-of select="translate(COL20,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
						</xsl:variable>


						<!--<xsl:variable name ="varCallPutCode">
							<xsl:choose>
								<xsl:when test="$varCALLPUT !='' and translate($varCALLPUT,$vLowercaseChars_CONST,$vUppercaseChars_CONST)='C'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="$varCALLPUT !='' and translate($varCALLPUT,$vLowercaseChars_CONST,$vUppercaseChars_CONST)='P'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
            
            <xsl:variable name ="varOptExpMonth">
							<xsl:choose>
								<xsl:when test="$varCALLPUT !=''">
									<xsl:value-of select ="substring(COL19,5,2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="concat($varCallPutCode,$varOptExpMonth)" />
							</xsl:call-template>
						</xsl:variable>

            <xsl:variable name="Strike">
              <xsl:choose>
                <xsl:when test="$varCALLPUT !=''">
                  <xsl:variable name ="varStrikeDecimal" select ="substring-after(COL18,'.')"/>
                  <xsl:variable name ="varStrikeInt" select ="substring-before(COL18,'.')"/>
                  <xsl:choose>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                      <xsl:value-of select ="concat(COL18,'0')"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                      <xsl:value-of select ="COL18"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                      <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="concat(COL18,'.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpYear">
							<xsl:choose>
								<xsl:when test="$varCALLPUT!=''">
									<xsl:value-of select ="substring(COL19,3,2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

            <xsl:variable name ="varRoot">
              <xsl:choose>
                <xsl:when test ="$varCALLPUT != ''">
                  <xsl:value-of select ="substring-before(COL2,'\')"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "BlankCount_Root" >
              <xsl:call-template name="noofBlanks">
                <xsl:with-param name="count1" select="(6) - string-length($varRoot)" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name ="varIntStr">
              <xsl:variable name ="varStrikeInt" select ="substring-before(COL18,'.')"/>
              <xsl:choose>
                <xsl:when test =" $varStrikeInt = ''">
                  <xsl:value-of select ="COL18"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varStrikeInt"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "Zeros_IntStr" >
              <xsl:call-template name="noofzeros">
                <xsl:with-param name="count" select="(5) - string-length($varIntStr)" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name ="varDecimalStr">
              <xsl:variable name ="varStrikeDecimal" select ="substring-after(COL18,'.')"/>
              <xsl:choose>
                <xsl:when test ="$varStrikeDecimal = ''">
                  <xsl:value-of select ="COL18"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varStrikeDecimal"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name = "Zeros_DecimalStr" >
              <xsl:call-template name="noofzeros">
                <xsl:with-param name="count" select="(3) - string-length($varDecimalStr)" />
              </xsl:call-template>
            </xsl:variable>
            

						<!--<xsl:variable name="PB_SYMBOL" select="COL2"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>--><!--
                <IDCOOptionSymbol>
                  <xsl:value-of select ="''"/>
                </IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test="$varCALLPUT != ''">-->
                <Symbol>
                  <xsl:value-of select="COL2"/>
                </Symbol>
                <!--<IDCOOptionSymbol>
                  --><!--<xsl:value-of select ="concat('O:',translate(COL21,$vLowercaseChars_CONST,$vUppercaseChars_CONST),' ',$ExpYear,$varMonthCode,$Strike)"/>--><!--
                  <xsl:value-of select ="concat($varRoot,$BlankCount_Root,substring(COL19,3,2),substring(COL19,5,2),substring(COL19,7,2),$varCALLPUT,$Zeros_IntStr,$varIntStr,$Zeros_DecimalStr,$varDecimalStr,'U')"/>
                </IDCOOptionSymbol>
							</xsl:when>-->
							<!--<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="translate(COL2,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</Symbol>-->
                <!--<IDCOOptionSymbol>
                  <xsl:value-of select ="''"/>
                </IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose >-->


						<PBSymbol>
              <xsl:value-of select ="COL2"/>
						</PBSymbol>

						<!--<PBAssetType>
							<xsl:value-of select="$varCALLPUT"/>
						</PBAssetType>-->

						<xsl:choose>
							<xsl:when test="COL9 &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL9"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL9 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL9*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<xsl:variable name ="varSide">
							<xsl:value-of select="translate(COL1,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varSide = 'B' and $varCALLPUT='*'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S'and $varCALLPUT='*'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS' and $varCALLPUT='*'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='BTC' and $varCALLPUT=''">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide = 'B' and $varCALLPUT != ''">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S'and $varCALLPUT != ''">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<!--<xsl:when test="$varSide='SS'and $varCALLPUT != ''">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>-->
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL10 = 'tradedate' or COL10='*'">
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<!--date format YYYYMMDD-->
									<xsl:value-of select="concat(substring(COL10,5,2),'/',substring(COL10,7,2),'/', substring(COL10,1,4))"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL15))">
								<Commission>
									<xsl:value-of select="COL15"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL11))">
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
						

						<xsl:variable name="PB_CounterParty" select="COL12"/>
						<xsl:variable name="PRANA_CounterPartyCode">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='ML']/BrokerData[translate(@MLBroker,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_CounterParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_CounterPartyCode !=''">
								<CounterPartyID>
									<xsl:value-of select="$PRANA_CounterPartyCode"/>
								</CounterPartyID>
							</xsl:when>
							
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select="0"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>
					</PositionMaster>
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
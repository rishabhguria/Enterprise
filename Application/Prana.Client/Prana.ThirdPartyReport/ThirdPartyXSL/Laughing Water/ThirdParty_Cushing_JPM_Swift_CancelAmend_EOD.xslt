<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="GetBICCode">
    <xsl:param name="CountryName"/>
    <xsl:param name="CounterParty"/>
    <xsl:choose>
      <xsl:when test="$CounterParty = 'WCHV'">
        <xsl:choose>
          <xsl:when test="$CountryName = 'China'">
            <xsl:value-of select="'HSBCHKHHSEC'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Belgium'">
            <xsl:value-of select="'PARBFRPP'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Portugal'">
            <xsl:value-of select="'PARBFRPP'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Netherlands'">
            <xsl:value-of select="'PARBFRPP'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'NewZealand'">
            <xsl:value-of select="'NATANZ22'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'SouthAfrica'">
            <xsl:value-of select="'SBZAZAJJ'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Denmark'">
            <xsl:value-of select="'ESSEDKKK'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Spain'">
            <xsl:value-of select="'SABNESMMSSS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Germany'">
            <xsl:value-of select="'PARBDEFF'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Australia'">
            <xsl:value-of select="'CHASAU2XCCS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Finland'">
            <xsl:value-of select="'ESSEFIHX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Mexico'">
            <xsl:value-of select="'CITIUS33MER'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Italy'">
            <xsl:value-of select="'PARBITMM'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Japan'">
            <xsl:value-of select="'MHCBJPJ2'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Norway'">
            <xsl:value-of select="'ESSENOKX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'France'">
            <xsl:value-of select="'PARBFRPP'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Canada'">
            <xsl:value-of select="'ROYCCAT2SET'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Singapore'">
            <xsl:value-of select="'DEUTSGSGCUS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Switzerland'">
            <xsl:value-of select="'UBSWCHZH80A'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Sweden'">
            <xsl:value-of select="'ESSESESS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Argentina'">
            <xsl:value-of select="'CITIUS33ARR'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'United States'">
            <xsl:value-of select="'PNBPUS3CLBR'"/>
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$CountryName = 'Argentina'">
            <xsl:value-of select="'BSSCHARBASSS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Austria'">
            <xsl:value-of select="'DEUTATWWCUS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Australia'">
            <xsl:value-of select="'CITIAU3X'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Belgium'">
            <xsl:value-of select="'CITTGB2L'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Brazil'">
            <xsl:value-of select="'BNIFBRRJ'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Canada'">
            <xsl:value-of select="'CITICATT'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Denmark'">
            <xsl:value-of select="'NDEADKKK'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Finland'">
            <xsl:value-of select="'NDEAFIHH'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'France'">
            <xsl:value-of select="'CITTGB2L'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Germany'">
            <xsl:value-of select="'DEUTDEFFCUS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Greece'">
            <xsl:value-of select="'CITIGRAA'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'HongKong'">
            <xsl:value-of select="'SCBLHKHH'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Hungary'">
            <xsl:value-of select="'INGBHUHB'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Iceland'">
            <xsl:value-of select="'ARIOISRE'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Indonesia'">
            <xsl:value-of select="'SCBLIDJX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Israel'">
            <xsl:value-of select="'LUMIILIT'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Italy'">
            <xsl:value-of select="'CITIITMX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Japan'">
            <xsl:value-of select="'CITIJPJT'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Malaysia'">
            <xsl:value-of select="'SCBLMYKX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Mexico'">
            <xsl:value-of select="'CITIUS33MER'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Netherlands'">
            <xsl:value-of select="'CITTGB2L'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'NewZealand'">
            <xsl:value-of select="'CITINZ2X'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Norway'">
            <xsl:value-of select="'NDEANOKK'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Philippines'">
            <xsl:value-of select="'SCBLPHMM'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Poland'">
            <xsl:value-of select="'INGBPLPW'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Portugal'">
            <xsl:value-of select="'CITIPTPX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Singapore'">
            <xsl:value-of select="'SCBLSGSG'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'SouthAfrica'">
            <xsl:value-of select="'FIRNZAJJ'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'SouthKorea'">
            <xsl:value-of select="'SCBLKRSE'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Spain'">
            <xsl:value-of select="'PARBESMX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Sweden'">
            <xsl:value-of select="'NDEASESS'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Switzerland'">
            <xsl:value-of select="'CITICHZZ'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'Thailand'">
            <xsl:value-of select="'SCBLTHBX'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'United Kingdom'">
            <xsl:value-of select="'CRST/393'"/>
          </xsl:when>
          <xsl:when test="$CountryName = 'United States'">
            <xsl:value-of select="'JEFFUS33'"/>
          </xsl:when>
        </xsl:choose>

      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="GetCodeCounterParty">
    <xsl:param name="CounterParty">
      <xsl:choose>
        <xsl:when test="CounterParty = 'JEFF'">
          <xsl:value-of select="'JEFFUS33'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:param>
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

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:variable name="varBICCode">
    <xsl:value-of select="'NIRVANAXXXXX0000000000'"/>
  </xsl:variable>

  

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(format-number(OrderQty,'0.00') * format-number(AvgPrice,'0.00000') * AssetMultiplier) + (format-number(CommissionCharged,'0.00') + format-number(SoftCommissionCharged,'0.00') + format-number(OtherBrokerFees,'0.00') + format-number(ClearingBrokerFee,'0.00') + format-number(StampDuty,'0.00') + format-number(TransactionLevy,'0.00') + format-number(ClearingFee,'0.00') + format-number(TaxOnCommissions,'0.00') + format-number(MiscFees,'0.00') + format-number(SecFee,'0.00') + format-number(OccFee,'0.00') + format-number(OrfFee,'0.00'))"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(format-number(OrderQty,'0.00') * format-number(AvgPrice,'0.00000') * AssetMultiplier) - (format-number(CommissionCharged,'0.00') + format-number(SoftCommissionCharged,'0.00') + format-number(OtherBrokerFees,'0.00') + format-number(ClearingBrokerFee,'0.00') + format-number(StampDuty,'0.00') + format-number(TransactionLevy,'0.00') + format-number(ClearingFee,'0.00') + format-number(TaxOnCommissions,'0.00') + format-number(MiscFees,'0.00') + format-number(SecFee,'0.00') + format-number(OccFee,'0.00') + format-number(OrfFee,'0.00'))"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test ="TaxLotState !='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>
              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <FileFooter>
                <xsl:value-of select="'true'"/>
              </FileFooter>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>

              <xsl:variable name="varMessageType">
                <xsl:choose>
                  <xsl:when test="substring(Side,1,1) = 'B'">
                    <xsl:value-of select="541"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="543"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varTradeHour">
                <xsl:value-of select="substring-before(substring-after(TradeDate, 'T'),':')"/>
              </xsl:variable>

              <xsl:variable name="varTradeTime">
                <xsl:choose>
                  <xsl:when test="string-length($varTradeHour) = 1">
                    <xsl:value-of select="concat('0', $varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat($varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varTradeDate">
                <xsl:value-of select="concat(substring($TradeDate, 9, 2), substring($TradeDate, 1, 2), substring($TradeDate, 4, 2))"/>
              </xsl:variable>


              <xsl:variable name="PB_NAME" select="'JPM'"/>

              <xsl:variable name ="varCurrencyName">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varDTCCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyDTCMapping.xml')/CurrencyMapping/PB[@Name= $PB_NAME]/BrokerData[@PranaCurrency=$varCurrencyName]/@DTCCode"/>
              </xsl:variable>

              <xsl:variable name ="varAgentCountry">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varAgentBICMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@AgentBIC"/>
              </xsl:variable>

              <xsl:variable name ="varCoustodinBICMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@ClearingBIC"/>
              </xsl:variable>

              <xsl:variable name ="varPlaceOfSettlementMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@PSET"/>
              </xsl:variable>

           

              <Block1>
                <xsl:value-of select="concat('{1:F01CHASEUS33MAA0579041082}','{2:O', $varMessageType, $varTradeTime, $varTradeDate, $varBICCode, $varTradeDate, $varTradeTime, 'N}{4:')"/>
              </Block1>

              <Seq_A1>

                <xsl:value-of select="':16R:GENL'"/>
              </Seq_A1>


              <Seq_A2>

                <xsl:value-of select="concat(':20C','::SEME//',EntityID)"/>
              </Seq_A2>

              <xsl:variable name="varTaxlotStateTx">
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'NEWM'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of  select="'CANC'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_A3>

                <xsl:value-of select="concat(':23G:',$varTaxlotStateTx)"/>
              </Seq_A3>

             

              <Seq_A4>

                <xsl:value-of select="':16S:GENL'"/>
              </Seq_A4>


              <Seq_A5>

                <xsl:value-of select="':16R:TRADDET'"/>
              </Seq_A5>

              <xsl:variable name="varFomateSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varSettleDate">
                <xsl:value-of select="concat(substring-after(substring-after($varFomateSettlementDate,'/'),'/'),substring-before($varFomateSettlementDate,'/'),substring-before(substring-after($varFomateSettlementDate,'/'),'/'))"/>
              </xsl:variable>
              <Seq_A6>

                <xsl:value-of select="concat(':98A::SETT//', $varSettleDate)"/>
              </Seq_A6>

              <xsl:variable name="varFomateTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varSTradeDate">
                <xsl:value-of select="concat(substring-after(substring-after($varFomateTradeDate,'/'),'/'),substring-before($varFomateTradeDate,'/'),substring-before(substring-after($varFomateTradeDate,'/'),'/'))"/>
              </xsl:variable>
              <Seq_A7>

                <xsl:value-of select="concat(':98A::TRAD//', $varSTradeDate)"/>
              </Seq_A7>

              <xsl:variable name="varAvgPrice">
                <xsl:value-of select="format-number(AvgPrice,'0.00000')"/>
              </xsl:variable>
                

              <xsl:variable name="varDealPrice">
                <xsl:choose>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="concat(':90B::DEAL//PRCT/',CurrencySymbol,substring-before($varAvgPrice,'.'),',',substring-after($varAvgPrice, '.'))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(':90B::DEAL//ACTU/',CurrencySymbol,substring-before($varAvgPrice,'.'),',',substring-after($varAvgPrice, '.'))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_A8>

                <xsl:value-of select="$varDealPrice"/>
              </Seq_A8>


              <Seq_B1>

                <xsl:value-of select="concat(':35B:ISIN ', ISIN)"/>
              </Seq_B1>


              <Seq_B2>

                <xsl:value-of select="CompanyName"/>
              </Seq_B2>


              <!--<Seq_BR3>

                <xsl:value-of select="':16R:FIA'"/>
              </Seq_BR3>

              <Seq_B12A>

                <xsl:value-of select="':12A::CLAS/ISIT/CS'"/>
              </Seq_B12A>

              <Seq_BS4>

                <xsl:value-of select="':16S:FIA'"/>
              </Seq_BS4>-->

              <Seq_B3>
                <xsl:value-of select="':16S:TRADDET'"/>
              </Seq_B3>

              <Seq_B4>
                <xsl:value-of select="':16R:FIAC'"/>
              </Seq_B4>


              <xsl:variable name="varTaxlotQuantity">
                <xsl:value-of select="concat(substring-before(format-number(OrderQty,'0.00'),'.'),',',substring-after(format-number(OrderQty,'0.00'), '.'))"/>
              </xsl:variable>
              <Seq_B5>

                <xsl:value-of select="concat(':36B::SETT//UNIT/',$varTaxlotQuantity)"/>
              </Seq_B5>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <xsl:variable name="varAccounName">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_E1>
                <xsl:value-of select="concat(':97A::SAFE//',$varAccounName)"/>
              </Seq_E1>


              <Seq_E2>
                <xsl:value-of select="':16S:FIAC'"/>
              </Seq_E2>


              <Seq_E3>
                <xsl:value-of select="':16R:SETDET'"/>
              </Seq_E3>


              <Seq_E4>

                <xsl:value-of select="':22F::SETR//TRAD'"/>
              </Seq_E4>

              <!--<Seq_E5>
               
                <xsl:value-of select="':22F::BLOC//BLPA'"/>
              </Seq_E5>-->


              <Seq_E6>
                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E6>

              
              <xsl:variable name="varAgent">
                <xsl:choose>
                  <xsl:when test="substring(Side, 1, 1) = 'B'">
                    <xsl:value-of select="'DEAG'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'REAG'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
			  
              <Seq_E7>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="concat(':95R::',$varAgent,'/','DTCYID','/','00000005')"/>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='CAD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>                    
                  </xsl:when>
                 

                  <xsl:when test="CurrencySymbol=' JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>                    
                  </xsl:when>

                  <!-- <xsl:when test="CurrencySymbol='GBP'"> -->
                    <!-- <xsl:choose> -->
                      <!-- <xsl:when test="substring(Side, 1, 1) = 'B'"> -->
                        <!-- <xsl:value-of select="concat(':95R::',$varAgent,'/',$varAgentBICMapping,'/',$varDTCCode)"/> -->
                      <!-- </xsl:when> -->
                      <!-- <xsl:otherwise> -->
                        <!-- <xsl:value-of select="concat(':95R::',$varAgent,'/',$varAgentBICMapping,'/',$varDTCCode)"/> -->
                      <!-- </xsl:otherwise> -->
                    <!-- </xsl:choose> -->
                  <!-- </xsl:when> -->
				  
				   <xsl:when test="CurrencySymbol='GBP'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
				  
				   <xsl:when test="CurrencySymbol='HKD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
				  
				    <xsl:when test="CurrencySymbol='AUD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <!--<xsl:when test="CurrencySymbol='EUR'">
                    <xsl:value-of select="concat(':95R::',$varAgent,'/',$varAgentBICMapping,'/',$varDTCCode)"/>
                  </xsl:when>-->
                  
                  <xsl:otherwise>
                    <!-- <xsl:value-of select="concat(':95R::',$varAgent,'/',$varAgentBICMapping,'/',$varDTCCode)"/> -->
					<xsl:value-of select="concat(':95R::',$varAgent,'/','/',$varDTCCode)"/>
                  </xsl:otherwise>
                </xsl:choose>               
              </Seq_E7>

              <Seq_E16S>
                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E16S>


              <Seq_E8>

                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E8>

              <xsl:variable name="varAgentType">
                <xsl:choose>
                  <xsl:when test="substring(Side, 1, 1) = 'B'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'BUYR'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              

              <Seq_E9>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select ="concat(':95R::', $varAgentType ,'/','DTCYID','/','00000005')"/>
                  </xsl:when>
                  
                  <xsl:when test="CurrencySymbol='CAD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='GBP'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  
                  <xsl:otherwise>
                    <xsl:value-of select ="concat(':95P::', $varAgentType ,'/',$varCoustodinBICMapping,'/',$varDTCCode)"/>
                  </xsl:otherwise>
                </xsl:choose>                
              </Seq_E9>


              <Seq_E10>
                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E10>


              <Seq_E11>

                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E11>

             
              <Seq_E12>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="concat(':95P::','PSET', '//','DTCYUS33')"/> 
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(':95P::','PSET', '//',$varPlaceOfSettlementMapping)"/>
                  </xsl:otherwise>
                </xsl:choose>
                <!--<xsl:value-of select="concat(':95R::', $varReverseSide, '//', $varSenderBICCode)"/>-->
                <!--<xsl:value-of select="concat(':95P::','PSET', '//','DTCYID','/',$varDTCCode)"/>-->
              
              </Seq_E12>


              <Seq_E13>

                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E13>


              <Seq_E14>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E14>

              <xsl:variable name="varPrincipalAmount">
                <xsl:value-of select="(format-number(OrderQty,'0.00') * format-number(AvgPrice,'0.00000') * AssetMultiplier)"/>
              </xsl:variable>


              <xsl:variable name="varSeq19A">
                <xsl:value-of select="format-number($varPrincipalAmount,'0.00')"/> 
              </xsl:variable>

              <Seq_E15>

                <xsl:value-of select="concat(':19A::DEAL//', CurrencySymbol, substring-before($varSeq19A,'.'),',',substring-after($varSeq19A, '.'))"/>
              </Seq_E15>


              <Seq_E16>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E16>


              <Seq_E17>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E17>

              <xsl:variable name="varCommissionFee">
                <xsl:value-of select="format-number((format-number(CommissionCharged,'0.00') + format-number(OtherBrokerFees,'0.00') + format-number(StampDuty,'0.00') + format-number(TransactionLevy,'0.00') + format-number(ClearingFee,'0.00') + format-number(TaxOnCommissions,'0.00') + format-number(MiscFees,'0.00')),'0.00')"/>
              </xsl:variable>

              <Seq_E18>

                <xsl:value-of select="concat(':19A::ACRU//', CurrencySymbol,'0',',','0')"/>               
              </Seq_E18>


              <Seq_E161>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E161>


              <Seq_E171>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E171>


              <Seq_E181>

                <xsl:value-of select="concat(':19A::EXEC//', CurrencySymbol, substring-before($varCommissionFee,'.'),',',substring-after($varCommissionFee,'.'))"/>
              </Seq_E181>

              <Seq_E19>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E19>


               <xsl:variable name="varSecFee">
                <xsl:value-of select="format-number((SecFee),'0.00')"/>
              </xsl:variable>
              <Seq_E31>
                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E31>

              <Seq_E32>
                <xsl:value-of select="concat(':19A::REGF//', CurrencySymbol, substring-before($varSecFee,'.'),',',substring-after($varSecFee,'.'))"/>
              </Seq_E32>

              <Seq_E33>
                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E33>


              <Seq_E20>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E20>

              <xsl:variable name="varNetAmount">
                <xsl:value-of select="format-number($varNetamount,'0.00')"/>
              </xsl:variable>

              <Seq_E21>

                <xsl:value-of select="concat(':19A::SETT//',CurrencySymbol, substring-before($varNetAmount,'.'),',',substring-after($varNetAmount,'.'))"/>
              </Seq_E21>


              <Seq_E22>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E22>


              <Seq_E23>

                <xsl:value-of select="':16S:SETDET'"/>
              </Seq_E23>


              <Seq_E24>

                <xsl:value-of select="'-}'"/>
              </Seq_E24>



              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

                <RowHeader>
                  <xsl:value-of select ="'false'"/>
                </RowHeader>
                <FileHeader>
                  <xsl:value-of select="'true'"/>
                </FileHeader>

                <FileFooter>
                  <xsl:value-of select="'true'"/>
                </FileFooter>

                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>

                <xsl:variable name="varMessageType">
                  <xsl:choose>
                    <xsl:when test="substring(Side,1,1) = 'B'">
                      <xsl:value-of select="541"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="543"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varTradeHour">
                  <xsl:value-of select="substring-before(substring-after(TradeDate, 'T'),':')"/>
                </xsl:variable>

                <xsl:variable name="varTradeTime">
                  <xsl:choose>
                    <xsl:when test="string-length($varTradeHour) = 1">
                      <xsl:value-of select="concat('0', $varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat($varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="TradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="TradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>

                <xsl:variable name="varTradeDate">
                  <xsl:value-of select="concat(substring($TradeDate, 9, 2), substring($TradeDate, 1, 2), substring($TradeDate, 4, 2))"/>
                </xsl:variable>


                <xsl:variable name="PB_NAME" select="'JPM'"/>

                <xsl:variable name ="varCurrencyName">
                  <xsl:value-of select ="CurrencySymbol"/>
                </xsl:variable>

                <xsl:variable name ="varDTCCode">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyDTCMapping.xml')/CurrencyMapping/PB[@Name= $PB_NAME]/BrokerData[@PranaCurrency=$varCurrencyName]/@DTCCode"/>
                </xsl:variable>


                <xsl:variable name ="varAgentCountry">
                  <xsl:value-of select ="CurrencySymbol"/>
                </xsl:variable>

                <xsl:variable name ="varAgentBICMapping">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@AgentBIC"/>
                </xsl:variable>

                <xsl:variable name ="varCoustodinBICMapping">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@ClearingBIC"/>
                </xsl:variable>

                <xsl:variable name ="varPlaceOfSettlementMapping">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@PSET"/>
                </xsl:variable>

           
                <Block1>
                  <xsl:value-of select="concat('{1:F01CHASEUS33MAA0579041082}','{2:O', $varMessageType, $varTradeTime, $varTradeDate, $varBICCode, $varTradeDate, $varTradeTime, 'N}{4:')"/>
                </Block1>

                <Seq_A1>

                  <xsl:value-of select="':16R:GENL'"/>
                </Seq_A1>


                <Seq_A2>

                  <xsl:value-of select="concat(':20C','::SEME//',EntityID)"/>
                </Seq_A2>

                <xsl:variable name="varTaxlotStateTx">
                  <xsl:value-of  select="'CANC'"/>
                </xsl:variable>
                <Seq_A3>

                  <xsl:value-of select="concat(':23G:',$varTaxlotStateTx)"/>
                </Seq_A3>

                <!--<Seq_B99>
                  
                  <xsl:value-of select="':99B::TOSE//'"/>
                </Seq_B99>

                <Seq_B_B99>
                  
                  <xsl:value-of select="':99B::SETT//'"/>
                </Seq_B_B99>-->

                <!--<Seq_A9>

                  <xsl:value-of select="':16R:LINK'"/>
                </Seq_A9>-->


                <!--<xsl:variable name="varSeq20C">
                  <xsl:value-of select="concat(':20C::PREV//',EntityID)"/>
                </xsl:variable>
                <Seq_20C>

                  <xsl:value-of select="$varSeq20C"/>
                </Seq_20C>-->

                <!--<Seq_A41>

                  <xsl:value-of select="':16S:LINK'"/>
                </Seq_A41>-->

                <Seq_A4>

                  <xsl:value-of select="':16S:GENL'"/>
                </Seq_A4>


                <Seq_A5>

                  <xsl:value-of select="':16R:TRADDET'"/>
                </Seq_A5>

                <xsl:variable name="varFormateSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>

                <xsl:variable name="varSettleDate">
                  <xsl:value-of select="concat(substring-after(substring-after($varFormateSettlementDate,'/'),'/'),substring-before($varFormateSettlementDate,'/'),substring-before(substring-after($varFormateSettlementDate,'/'),'/'))"/>
                </xsl:variable>
                <Seq_A6>

                  <xsl:value-of select="concat(':98A::SETT//', $varSettleDate)"/>
                </Seq_A6>

                <xsl:variable name="varFormateTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>

                <xsl:variable name="varSTradeDate">
                  <xsl:value-of select="concat(substring-after(substring-after($varFormateTradeDate,'/'),'/'),substring-before($varFormateTradeDate,'/'),substring-before(substring-after($varFormateTradeDate,'/'),'/'))"/>
                </xsl:variable>
                <Seq_A7>

                  <xsl:value-of select="concat(':98A::TRAD//', $varTradeDate)"/>
                </Seq_A7>

                <xsl:variable name="varOldAvgPrice">
                  <xsl:value-of select="format-number(OldAvgPrice,'0.00000')"/>
                </xsl:variable>


                <xsl:variable name="varDealPrice">
                  <xsl:choose>
                    <xsl:when test="Asset='FixedIncome'">
                      <xsl:value-of select="concat(':90B::DEAL//PRCT/',CurrencySymbol,substring-before($varOldAvgPrice,'.'),',',substring-after($varOldAvgPrice, '.'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(':90B::DEAL//ACTU/',CurrencySymbol,substring-before($varOldAvgPrice,'.'),',',substring-after($varOldAvgPrice, '.'))"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <Seq_A8>

                  <xsl:value-of select="$varDealPrice"/>
                </Seq_A8>


                <Seq_B1>

                  <xsl:value-of select="concat(':35B:ISIN ', ISIN)"/>
                </Seq_B1>


                <Seq_B2>

                  <xsl:value-of select="CompanyName"/>
                </Seq_B2>



                <Seq_B3>

                  <xsl:value-of select="':16S:TRADDET'"/>
                </Seq_B3>


                <Seq_B4>

                  <xsl:value-of select="':16R:FIAC'"/>
                </Seq_B4>
                
                <xsl:variable name="varTaxlotQuantity">
                  <xsl:value-of select="concat(substring-before(format-number(OldExecutedQuantity,'0.00'),'.'),',',substring-after(format-number(OldExecutedQuantity,'0.00'), '.'))"/>
                </xsl:variable>

                <Seq_B5>

                  <xsl:value-of select="concat(':36B::SETT//UNIT/',$varTaxlotQuantity)"/>
                </Seq_B5>

                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountName"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>

                <xsl:variable name="varAccounName">
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                      <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_FUND_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <Seq_E1>

                  <xsl:value-of select="concat(':97A::SAFE//',$varAccounName)"/>
                </Seq_E1>


                <Seq_E2>

                  <xsl:value-of select="':16S:FIAC'"/>
                </Seq_E2>


                <Seq_E3>

                  <xsl:value-of select="':16R:SETDET'"/>
                </Seq_E3>


                <Seq_E4>

                  <xsl:value-of select="':22F::SETR//TRAD'"/>
                </Seq_E4>

                <Seq_E6>

                  <xsl:value-of select="':16R:SETPRTY'"/>
                </Seq_E6>


                <xsl:variable name="varAgent">
                  <xsl:choose>
                    <xsl:when test="substring(OldSide, 1, 1) = 'B'">
                      <xsl:value-of select="'DEAG'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'REAG'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <Seq_E7>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD'">
                      <xsl:value-of select="concat(':95R::',$varAgent,'/','DTCYID','/','00000005')"/>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='CAD'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>


                    <xsl:when test="CurrencySymbol=' JPY'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='GBP'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
					
					 <xsl:when test="CurrencySymbol='HKD'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
					
					 <xsl:when test="CurrencySymbol='AUD'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <!--<xsl:when test="CurrencySymbol='EUR'">
                      <xsl:value-of select="concat(':95R::',$varAgent,'/',$varAgentBICMapping,'/',$varDTCCode)"/>
                    </xsl:when>-->

                    <xsl:otherwise>
                      <xsl:value-of select="concat(':95R::',$varAgent,'/','/',$varDTCCode)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Seq_E7>

             

                <Seq_E16S>
                  <xsl:value-of select="':16S:SETPRTY'"/>
                </Seq_E16S>


                <Seq_E8>

                  <xsl:value-of select="':16R:SETPRTY'"/>
                </Seq_E8>

                <xsl:variable name="varAgentType">
                  <xsl:choose>
                    <xsl:when test="substring(OldSide, 1, 1) = 'B'">
                      <xsl:value-of select="'SELL'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'BUYR'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


             
                <Seq_E9>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD'">
                      <xsl:value-of select ="concat(':95R::', $varAgentType ,'/','DTCYID','/','00000005')"/>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='CAD'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='JPY'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='GBP'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <xsl:when test="CurrencySymbol='JPY'">
                      <xsl:choose>
                        <xsl:when test="substring(Side, 1, 1) = 'B'">
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select ="concat(':95P::', $varAgentType ,'/',$varCoustodinBICMapping,'/',$varDTCCode)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Seq_E9>


                <Seq_E10>

                  <xsl:value-of select="':16S:SETPRTY'"/>
                </Seq_E10>


                <Seq_E11>

                  <xsl:value-of select="':16R:SETPRTY'"/>
                </Seq_E11>

                <xsl:variable name="varReverseSide">
                  <xsl:choose>
                    <xsl:when test="substring(OldSide, 1, 1) = 'B'">
                      <xsl:value-of select="'BUYR'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'SELL'"/>

                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <Seq_E12>

                  <!--<xsl:value-of select="concat(':95R::', $varReverseSide, '//', $varSenderBICCode)"/>-->
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD'">
                      <xsl:value-of select="concat(':95P::','PSET', '//','DTCYUS33')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(':95P::','PSET', '//',$varPlaceOfSettlementMapping)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Seq_E12>


                <Seq_E13>

                  <xsl:value-of select="':16S:SETPRTY'"/>
                </Seq_E13>


                <Seq_E14>

                  <xsl:value-of select="':16R:AMT'"/>
                </Seq_E14>

                <xsl:variable name="varPrincipalAmount">
                  <xsl:value-of select="(format-number(OldExecutedQuantity,'0.00') * format-number(OldAvgPrice,'0.00000') * AssetMultiplier)"/>
                </xsl:variable>


                <xsl:variable name="varOldSeq19A">
                  <xsl:value-of select="format-number($varPrincipalAmount,'0.00')"/> 
                </xsl:variable>

                <Seq_E15>

                  <xsl:value-of select="concat(':19A::DEAL//', CurrencySymbol, substring-before($varOldSeq19A,'.'),',',substring-after($varOldSeq19A, '.'))"/>
                </Seq_E15>


                <Seq_E16>

                  <xsl:value-of select="':16S:AMT'"/>
                </Seq_E16>


                <Seq_E17>

                  <xsl:value-of select="':16R:AMT'"/>
                </Seq_E17>

                <xsl:variable name="varCommissionFee">
                  <xsl:value-of select="format-number((format-number(OldCommission,'0.00') + format-number(OldOtherBrokerFees,'0.00') + format-number(OldStampDuty,'0.00') + format-number(OldTransactionLevy,'0.00') + format-number(OldClearingFee,'0.00') + format-number(OldTaxOnCommissions,'0.00') + format-number(OldMiscFees,'0.00')),'0.00')"/>
                </xsl:variable>
                
                <Seq_E18>

                  <xsl:value-of select="concat(':19A::ACRU//', CurrencySymbol,'0',',','0')"/>
                </Seq_E18>

                <Seq_E161>

                  <xsl:value-of select="':16S:AMT'"/>
                </Seq_E161>


                <Seq_E171>

                  <xsl:value-of select="':16R:AMT'"/>
                </Seq_E171>


                <Seq_E181>

                  <xsl:value-of select="concat(':19A::EXEC//', CurrencySymbol, substring-before($varCommissionFee,'.'),',',substring-after($varCommissionFee,'.'))"/>
                </Seq_E181>

                <Seq_E19>

                  <xsl:value-of select="':16S:AMT'"/>
                </Seq_E19>


           <xsl:variable name="varSecFee">
                <xsl:value-of select="format-number((OldSecFee),'0.00')"/>
              </xsl:variable>
              <Seq_E31>
                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E31>

              <Seq_E32>
                <xsl:value-of select="concat(':19A::REGF//', CurrencySymbol, substring-before($varSecFee,'.'),',',substring-after($varSecFee,'.'))"/>
              </Seq_E32>

              <Seq_E33>
                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E33>

                <Seq_E20>

                  <xsl:value-of select="':16R:AMT'"/>
                </Seq_E20>

                <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(format-number(OldExecutedQuantity,'0.00') * format-number(OldAvgPrice,'0.00000') * AssetMultiplier) + (format-number(OldCommission,'0.00') + format-number(OldSoftCommission,'0.00') + format-number(OldOtherBrokerFees,'0.00') + format-number(OldClearingBrokerFee,'0.00') + format-number(OldStampDuty,'0.00') + format-number(OldTransactionLevy,'0.00') + format-number(OldClearingFee,'0.00') + format-number(OldTaxOnCommissions,'0.00') + format-number(OldMiscFees,'0.00') + format-number(OldSecFee,'0.00') + format-number(OldOccFee,'0.00') + format-number(OldOrfFee,'0.00'))"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(format-number(OldExecutedQuantity,'0.00') * format-number(OldAvgPrice,'0.00') * AssetMultiplier) - (format-number(OldCommission,'0.00') + format-number(OldSoftCommission,'0.00') + format-number(OldOtherBrokerFees,'0.00') + format-number(OldClearingBrokerFee,'0.00') + format-number(OldStampDuty,'0.00') + format-number(OldTransactionLevy,'0.00') + format-number(OldClearingFee,'0.00') + format-number(OldTaxOnCommissions,'0.00') + format-number(OldMiscFees,'0.00') + format-number(OldSecFee,'0.00') + format-number(OldOccFee,'0.00') + format-number(OldOrfFee,'0.00'))"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varNetAmount">                 
                  <xsl:value-of select="format-number($varOldNetAmount,'0.00')"/>
                </xsl:variable>

                <Seq_E21>

                  <xsl:value-of select="concat(':19A::SETT//',CurrencySymbol, substring-before($varNetAmount,'.'),',',substring-after($varNetAmount,'.'))"/>
                </Seq_E21>


                <Seq_E22>

                  <xsl:value-of select="':16S:AMT'"/>
                </Seq_E22>


                <Seq_E23>

                  <xsl:value-of select="':16S:SETDET'"/>
                </Seq_E23>


                <Seq_E24>

                  <xsl:value-of select="'-}'"/>
                </Seq_E24>



                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>
              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <FileFooter>
                <xsl:value-of select="'true'"/>
              </FileFooter>

              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>


              <xsl:variable name="varMessageType">
                <xsl:choose>
                  <xsl:when test="substring(Side,1,1) = 'B'">
                    <xsl:value-of select="541"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="543"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varTradeHour">
                <xsl:value-of select="substring-before(substring-after(TradeDate, 'T'),':')"/>
              </xsl:variable>

              <xsl:variable name="varTradeTime">
                <xsl:choose>
                  <xsl:when test="string-length($varTradeHour) = 1">
                    <xsl:value-of select="concat('0', $varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat($varTradeHour, substring(substring-after(TradeDate, ':'), 1, 2))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varTradeDate">
                <xsl:value-of select="concat(substring($TradeDate, 9, 2), substring($TradeDate, 1, 2), substring($TradeDate, 4, 2))"/>
              </xsl:variable>


              <xsl:variable name="PB_NAME" select="'JPM'"/>

              <xsl:variable name ="varCurrencyName">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varDTCCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CurrencyDTCMapping.xml')/CurrencyMapping/PB[@Name= $PB_NAME]/BrokerData[@PranaCurrency=$varCurrencyName]/@DTCCode"/>
              </xsl:variable>


              <xsl:variable name ="varAgentCountry">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varAgentBICMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@AgentBIC"/>
              </xsl:variable>

              <xsl:variable name ="varCoustodinBICMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@ClearingBIC"/>
              </xsl:variable>

              <xsl:variable name ="varPlaceOfSettlementMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='JPM']/InstructionData[@SettlementInstruction=$varAgentCountry]/@PSET"/>
              </xsl:variable>


              <Block1>
                <xsl:value-of select="concat('{1:F01CHASEUS33MAA0579041082}','{2:O', $varMessageType, $varTradeTime, $varTradeDate, $varBICCode, $varTradeDate, $varTradeTime, 'N}{4:')"/>
              </Block1>

              <Seq_A1>

                <xsl:value-of select="':16R:GENL'"/>
              </Seq_A1>


              <Seq_A2>

                <xsl:value-of select="concat(':20C','::SEME//',EntityID)"/>
              </Seq_A2>

              <xsl:variable name="varTaxlotStateTx">
                <xsl:value-of select ="'NEWM'"/>
              </xsl:variable>
              <Seq_A3>

                <xsl:value-of select="concat(':23G:',$varTaxlotStateTx)"/>
              </Seq_A3>

            

              <Seq_A4>

                <xsl:value-of select="':16S:GENL'"/>
              </Seq_A4>


              <Seq_A5>

                <xsl:value-of select="':16R:TRADDET'"/>
              </Seq_A5>

              <xsl:variable name="varFomateSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varSettleDate">
                <xsl:value-of select="concat(substring-after(substring-after($varFomateSettlementDate,'/'),'/'),substring-before($varFomateSettlementDate,'/'),substring-before(substring-after($varFomateSettlementDate,'/'),'/'))"/>
              </xsl:variable>
              <Seq_A6>

                <xsl:value-of select="concat(':98A::SETT//', $varSettleDate)"/>
              </Seq_A6>

              <xsl:variable name="varFomateTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varSTradeDate">
                <xsl:value-of select="concat(substring-after(substring-after($varFomateTradeDate,'/'),'/'),substring-before($varFomateTradeDate,'/'),substring-before(substring-after($varFomateTradeDate,'/'),'/'))"/>
              </xsl:variable>
              <Seq_A7>

                <xsl:value-of select="concat(':98A::TRAD//', $varSTradeDate)"/>
              </Seq_A7>

              <xsl:variable name="varAvgPrice">
                <xsl:value-of select="format-number(AvgPrice,'0.00000')"/>
              </xsl:variable>

              <xsl:variable name="varDealPrice">
                <xsl:choose>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="concat(':90B::DEAL//PRCT/',CurrencySymbol,substring-before($varAvgPrice,'.'),',',substring-after($varAvgPrice, '.'))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(':90B::DEAL//ACTU/',CurrencySymbol,substring-before($varAvgPrice,'.'),',',substring-after($varAvgPrice, '.'))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_A8>

                <xsl:value-of select="$varDealPrice"/>
              </Seq_A8>


              <Seq_B1>

                <xsl:value-of select="concat(':35B:ISIN ', ISIN)"/>
              </Seq_B1>


              <Seq_B2>

                <xsl:value-of select="CompanyName"/>
              </Seq_B2>



              <Seq_B3>

                <xsl:value-of select="':16S:TRADDET'"/>
              </Seq_B3>


              <Seq_B4>

                <xsl:value-of select="':16R:FIAC'"/>
              </Seq_B4>

              <xsl:variable name="varTaxlotQuantity">
                <xsl:value-of select="concat(substring-before(format-number(OrderQty,'0.00'),'.'),',',substring-after(format-number(OrderQty,'0.00'), '.'))"/>
              </xsl:variable>

              <Seq_B5>

                <xsl:value-of select="concat(':36B::SETT//UNIT/',$varTaxlotQuantity)"/>
              </Seq_B5>



              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <xsl:variable name="varAccounName">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_E1>

                <xsl:value-of select="concat(':97A::SAFE//',$varAccounName)"/>
              </Seq_E1>


              <Seq_E2>

                <xsl:value-of select="':16S:FIAC'"/>
              </Seq_E2>


              <Seq_E3>

                <xsl:value-of select="':16R:SETDET'"/>
              </Seq_E3>


              <Seq_E4>

                <xsl:value-of select="':22F::SETR//TRAD'"/>
              </Seq_E4>

           


              <Seq_E6>

                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E6>


              <xsl:variable name="varAgent">
                <xsl:choose>
                  <xsl:when test="substring(Side, 1, 1) = 'B'">
                    <xsl:value-of select="'DEAG'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'REAG'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Seq_E7>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="concat(':95R::',$varAgent,'/','DTCYID','/','00000005')"/>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='CAD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgent,'/','CDSL','/',$varAgentBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>


                  <xsl:when test="CurrencySymbol=' JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgent,'//',$varAgentBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='GBP'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>


                  <xsl:when test="CurrencySymbol='HKD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
				  
				  
				  <xsl:when test="CurrencySymbol='AUD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95R::',$varAgent,'/','CDSL','/',$varDTCCode)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
               

                  <xsl:otherwise>
                    <xsl:value-of select="concat(':95R::',$varAgent,'/','/',$varDTCCode)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Seq_E7>


              <Seq_E16S>

                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E16S>


              <Seq_E8>

                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E8>

              <xsl:variable name="varAgentType">
                <xsl:choose>
                  <xsl:when test="substring(Side, 1, 1) = 'B'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'BUYR'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <Seq_E9>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select ="concat(':95R::', $varAgentType ,'/','DTCYID','/','00000005')"/>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='CAD'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='GBP'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:when test="CurrencySymbol='JPY'">
                    <xsl:choose>
                      <xsl:when test="substring(Side, 1, 1) = 'B'">
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(':95P::',$varAgentType,'//',$varCoustodinBICMapping)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="concat(':95P::', $varAgentType ,'/',$varCoustodinBICMapping,'/',$varDTCCode)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Seq_E9>


              <Seq_E10>

                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E10>


              <Seq_E11>

                <xsl:value-of select="':16R:SETPRTY'"/>
              </Seq_E11>

              

              <Seq_E12>

                <!--<xsl:value-of select="concat(':95R::', $varReverseSide, '//', $varSenderBICCode)"/>-->
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="concat(':95P::','PSET', '//','DTCYUS33')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(':95P::','PSET', '//',$varPlaceOfSettlementMapping)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Seq_E12>


              <Seq_E13>

                <xsl:value-of select="':16S:SETPRTY'"/>
              </Seq_E13>


              <Seq_E14>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E14>

              <xsl:variable name="varPrincipalAmount">
                <xsl:value-of select="(format-number(OrderQty,'0.00') * format-number(AvgPrice,'0.00000') * AssetMultiplier)"/>
              </xsl:variable>


              <xsl:variable name="varSeq19A">
                <xsl:value-of select="format-number($varPrincipalAmount,'0.00')"/> 
              </xsl:variable>

              <Seq_E15>

                <xsl:value-of select="concat(':19A::DEAL//', CurrencySymbol, substring-before($varSeq19A,'.'),',',substring-after($varSeq19A, '.'))"/>
              </Seq_E15>


              <Seq_E16>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E16>


              <Seq_E17>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E17>

              <xsl:variable name="varCommissionFee">
                <xsl:value-of select="format-number((format-number(CommissionCharged,'0.00') + format-number(OtherBrokerFees,'0.00') + format-number(StampDuty,'0.00') + format-number(TransactionLevy,'0.00') + format-number(ClearingFee,'0.00') + format-number(TaxOnCommissions,'0.00') + format-number(MiscFees,'0.00')),'0.00')"/>
              </xsl:variable>
              
              <Seq_E18>

                <xsl:value-of select="concat(':19A::ACRU//', CurrencySymbol,'0',',','0')"/>
              </Seq_E18>

              <Seq_E161>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E161>


              <Seq_E171>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E171>


              <Seq_E181>

                <xsl:value-of select="concat(':19A::EXEC//', CurrencySymbol, substring-before($varCommissionFee,'.'),',',substring-after($varCommissionFee,'.'))"/>
              </Seq_E181>

              <Seq_E19>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E19>

             <xsl:variable name="varSecFee">
                <xsl:value-of select="format-number((SecFee),'0.00')"/>
              </xsl:variable>
              <Seq_E31>
                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E31>

              <Seq_E32>
                <xsl:value-of select="concat(':19A::REGF//', CurrencySymbol, substring-before($varSecFee,'.'),',',substring-after($varSecFee,'.'))"/>
              </Seq_E32>

              <Seq_E33>
                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E33>

              <Seq_E20>

                <xsl:value-of select="':16R:AMT'"/>
              </Seq_E20>

              <xsl:variable name="varNetAmount">             
                <xsl:value-of select="format-number($varNetamount,'0.00')"/>
              </xsl:variable>

              <Seq_E21>

                <xsl:value-of select="concat(':19A::SETT//',CurrencySymbol, substring-before($varNetAmount,'.'),',',substring-after($varNetAmount,'.'))"/>
              </Seq_E21>


              <Seq_E22>

                <xsl:value-of select="':16S:AMT'"/>
              </Seq_E22>


              <Seq_E23>

                <xsl:value-of select="':16S:SETDET'"/>
              </Seq_E23>


              <Seq_E24>

                <xsl:value-of select="'-}'"/>
              </Seq_E24>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>